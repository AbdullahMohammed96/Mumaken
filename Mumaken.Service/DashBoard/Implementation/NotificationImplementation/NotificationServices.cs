using AAITHelper.ModelHelper;
using AAITHelper;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Notification;
using Mumaken.Domain.ViewModel.Settings;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.NotificationInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAITHelper.Enums;
using Mumaken.Domain.Entities.UserTables;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;

namespace Mumaken.Service.DashBoard.Implementation.NotificationImplementation
{
    public class NotificationServices : INotificationServices
    {
        private readonly ApplicationDbContext _context;

        public NotificationServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<HistoryNotificationViewModel>> GetHistoryNotify()
        {
            var UserNotifies = await _context.HistoryNotify
                                            .OrderByDescending(c => c.Id)
                                             .Select(x => new HistoryNotificationViewModel
                                             {
                                                 Text = x.Text,
                                                 TextDate = x.Date,
                                                 NotifyCount=x.CountNotify,
                                             }).ToListAsync();
            return UserNotifies;
        }
        public async Task<List<GetAllNotificationViewModel>> GetAllNotification()
        {
            var allNotifications = await _context.Set<GetAllNotificationViewModel>()
               .FromSqlRaw("EXEC GetAllNotifications")
               .ToListAsync();
            return allNotifications;
            #region  With Query
            //var userNotification = await _context.NotifyUsers
            //    .Include(c => c.User)
            //    .Where(c => !c.User.IsDeleted && c.Type != (int)NotifyTypes.NotiyFromDashBord)
            //    .Select(c => new GetAllNotificationViewModel
            //    {
            //        Id=c.Id,
            //        UserName=c.User.user_Name,
            //        OrderId=c.OrderId,
            //        TextAr=c.TextAr,
            //        TextEn=c.TextEn,    
            //        DateSend=c.Date.ToString("MM/dd/yyyy HH:mm")
            //    })
            //    .ToListAsync();

            //var notifyDelegates=await _context.NotifyDelegts
            //   .Include(c => c.User)
            //   .Select(c => new GetAllNotificationViewModel
            //   {
            //       Id = c.Id,
            //       OrderId = c.OrderId,
            //       UserName = c.User.user_Name,
            //       TextAr = c.TextAr,
            //       TextEn = c.TextEn,
            //       DateSend = c.Date.ToString("MM/dd/yyyy HH:mm")

            //   }).ToListAsync();
            //var allNotifications = userNotification.Concat(notifyDelegates).ToList();
            #endregion
        }
        public async Task<List<UsersViewModel>> GetUsers()
        {
            var users = await _context.Users.Where(u => u.IsActive && u.TypeUser == (int)UserType.Client && !u.IsDeleted)
                                            .Select(x => new UsersViewModel
                                            {
                                                id = x.Id,
                                                name = x.user_Name
                                            }).ToListAsync();
            return users;
        }
        public async Task<List<UsersViewModel>> GetDeliveryCompany()
        {
            var Deleget = await _context.Users.Where(u => u.IsActive && u.TypeUser == (int)UserType.DeliverCompany && !u.IsDeleted)
                                            .Select(x => new UsersViewModel
                                            {
                                                id = x.Id,
                                                name = x.user_Name
                                            }).ToListAsync();
            return Deleget;
        }
        public async Task<List<UsersViewModel>> GetRentalCompany()
        {
            var Deleget = await _context.Users.Where(u => u.IsActive && u.TypeUser == (int)UserType.RentalCompany && !u.IsDeleted)
                                            .Select(x => new UsersViewModel
                                            {
                                                id = x.Id,
                                                name = x.user_Name
                                            }).ToListAsync();
            return Deleget;
        }
        public async Task<bool> Send(string msg, string employees, string deliveryCompanyes, string rentalCompanyes)
        {

            List<NotifyUser> NotifyUserers = new List<NotifyUser>();
            List<NotifyDelegt> NotifyDeliveryCompanyes = new List<NotifyDelegt>();
            List<NotifyDelegt> NotifyRentalCompanyes = new List<NotifyDelegt>();

            if (employees != null)
            {
                var employeeArr = employees.Split(',');
                foreach (var clientId in employeeArr)
                {
                    NotifyUserers.Add(new NotifyUser()
                    {
                        Date = HelperDate.GetCurrentDate(),
                        UserId = clientId,
                        Show = false,
                        TextAr = msg,
                        TextEn = msg,
                        Type = NotifyTypes.NotiyFromDashBord.ToNumber(),
                        OrderId = 0

                    });

                    await Notify(msg, msg, clientId, NotifyTypes.NotiyFromDashBord.ToNumber());

                }

                await _context.NotifyUsers.AddRangeAsync(NotifyUserers);
            }

            if (deliveryCompanyes != null)
            {
                var ProvidersArr = deliveryCompanyes.Split(',');
                foreach (var providerId in ProvidersArr)
                {
                    NotifyDeliveryCompanyes.Add(new NotifyDelegt()
                    {
                        Date = HelperDate.GetCurrentDate(),
                        UserId = providerId,
                        Show = false,
                        TextAr = msg,
                        TextEn = msg,
                        Type = NotifyTypes.NotiyFromDashBord.ToNumber(),
                        OrderId = 0
                    });

                    await Notify(msg, msg, providerId, NotifyTypes.NotiyFromDashBord.ToNumber());

                }

                await _context.NotifyDelegts.AddRangeAsync(NotifyDeliveryCompanyes);
            }
            if (rentalCompanyes != null)
            {
                var ProvidersArr = rentalCompanyes.Split(',');
                foreach (var providerId in ProvidersArr)
                {
                    NotifyRentalCompanyes.Add(new NotifyDelegt()
                    {
                        Date = HelperDate.GetCurrentDate(),
                        UserId = providerId,
                        Show = false,
                        TextAr = msg,
                        TextEn = msg,
                        Type = NotifyTypes.NotiyFromDashBord.ToNumber(),
                        OrderId = 0
                    });

                    await Notify(msg, msg, providerId, NotifyTypes.NotiyFromDashBord.ToNumber());

                }

                await _context.NotifyDelegts.AddRangeAsync(NotifyRentalCompanyes);
            }

            await _context.HistoryNotify.AddAsync(new()
            {
                Text = msg,
                Date = HelperDate.GetCurrentDate(),
                CountNotify = NotifyRentalCompanyes.Count + NotifyUserers.Count+ NotifyDeliveryCompanyes.Count
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Notify(string textAr, string textEn, string fkProvider, int stutes, int orderId = 0)
        {

            var TypeUser = await _context.Users.Where(x => x.Id == fkProvider).Select(x => x.TypeUser).FirstOrDefaultAsync();

            NotifyViewModel Fcm = await _context.Settings
                                                .Select(x => new NotifyViewModel
                                                {
                                                    AppId = x.ApplicationId,
                                                    SenderId = x.SenderId
                                                }).FirstOrDefaultAsync();

            List<DeviceIdModel> AllDeviceids = await _context.DeviceIds.Where(x => x.UserId == fkProvider).Select(x => new DeviceIdModel()
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName

            }).AsNoTracking().ToListAsync();
            //HelperFcm.SendPushNotification(Fcm.AppId, Fcm.SenderId, AllDeviceids, null, textAr, stutes, TypeUser, orderId);
            NotificationHelper.SendPushNotification(Fcm.AppId, Fcm.SenderId, AllDeviceids, null, textAr, stutes, orderId, 0);
            return true;
        }

    }
}
