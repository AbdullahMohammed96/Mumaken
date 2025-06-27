using AAITHelper.Enums;
using AutoMapper.QueryableExtensions;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.OrderDTO;
using Mumaken.Domain.Enums;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Logic;
using Mumaken.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mumaken.Domain.DTO;
using Mumaken.Domain.ViewModel.Orders;
using Azure.Core;
using Mumaken.Domain.Common.Helpers.ResponseHelper;
using AAITHelper;
using Mumaken.Domain.ViewModel.DeliveryCompanyOrder;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.ViewModel.AdminOrders;
using Mumaken.Domain.ViewModel.FinancialAccount;
using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Entities.SettingTables;
using AAITHelper.ModelHelper;
using Mumaken.Domain.DTO.SettingDto;
using Microsoft.AspNetCore.SignalR;

namespace Mumaken.Service.Api.Implementation.Logic
{
    public class OrderProvider : IOrderProvider
    {
        private readonly ApplicationDbContext _db;
        private readonly IOrderClient _orderClient;
        private readonly IHelper _helper;
        private readonly IHubContext<ChatHub> _hubContext;
        public OrderProvider(ApplicationDbContext db, IOrderClient orderClient, IHelper helper, IHubContext<ChatHub> hubContext)
        {
            _db = db;
            _orderClient = orderClient;
            _helper = helper;
            _hubContext = hubContext;
        }


        public async Task<BaseResponseDto<List<GetRentalOrdersByStatusViewModel>>> GetRentalCompanyOrders(int orderCase, string rentalCompanyId, string lang = "ar")
        {
            var rentalCompany = await _db.Users
                //.IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == rentalCompanyId);

            if (rentalCompany is null || !rentalCompany.IsAppravel)
            {
                return ResponseHelper.Failure<List<GetRentalOrdersByStatusViewModel>>(HelperMsg.creatMessage(lang, "جاري مراجعة بياناتك من الادارة", "Your data is being reviewed by the administration"));
            }
            var cultureInfo = Helper.GetCulture(lang);

            var query = _db.Orders
                .Include(c => c.RenewOrderdata)
                .Include(c => c.User)
                .Where(c => c.RentalCompanyId == rentalCompanyId
                        && c.OrderCase != (int)OrderCase.WaitAcceptAdministration);

            if (orderCase == (int)RentalCompanyOrder.newOdrer)
                query = query.Where(c => c.OrderCase == (int)OrderCase.WaitToAcceptRentalCompany);

            if (orderCase == (int)RentalCompanyOrder.Current)
                query = query.Where(c => c.OrderStatus == (int)OrderStutes.Current && !c.HasRenewToAdditionalPeriod);

            if (orderCase == (int)RentalCompanyOrder.RenewOrder)
                query = query.Where(c => c.HasRenewToAdditionalPeriod
                        && c.OrderStatus == (int)OrderStutes.Current && !c.IsCancelContract);

            if (orderCase == (int)RentalCompanyOrder.CancelContractOrder)
                query = query.Where(c => c.IsCancelContract);

            if (orderCase == (int)RentalCompanyOrder.Finished)
                query = query.Where(c => c.OrderStatus == (int)OrderStutes.Finished && !c.IsCancelContract);

            var orders = await query.Select(c => new GetRentalOrdersByStatusViewModel
            {
                OrderId = c.Id,
                DeliverName = c.User.user_Name,
                DeliverPhone = c.User.PhoneCode + " " + c.User.PhoneNumber,
                Price = c.HasRenewToAdditionalPeriod ? c.RenewOrderdata.OrderByDescending(c => c.Id).Select(c => c.NetPrice + c.BreakPeriodPrice + c.RentalConfirmationDelayPrice).FirstOrDefault()
                : c.NetPrice + c.BreakPeriodPrice + c.RentalConfirmationDelayPrice,
                CreationDate = c.CreationDate.ToString("dd/MM/yyyy", cultureInfo)
            }).OrderByDescending(c => c.OrderId).ToListAsync();

            return ResponseHelper.Success(orders);
        }
        public async Task<BaseResponseDto<object>> RefusedOrder(int orderId, string reasonForCanceled, string lang = "ar")
        {
            var order = await _db.Orders
                //.IgnoreQueryFilters()
                .Include(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Include(c => c.DeliverCompany)
                .FirstOrDefaultAsync(c => c.Id == orderId);
            if (order is not null)
            {
                order.OrderStatus = (int)OrderStutes.Refused;
                order.OrderCase = (int)OrderCase.Refused;
                foreach (var deliveryCompany in order.DeliverCompany)
                {
                    deliveryCompany.DeliverCompanyCase = (int)OrderCase.Refused;
                    //deliveryCompany.ReasonRefused = reasonForCanceled;
                }
                order.ReasonToCancled = reasonForCanceled;
                order.RejectionType = (int)RejectionType.RejectionFromAdmin;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                var notifiyUser = new NotifyUser
                {
                    OrderId = orderId,
                    UserId = order.UserId,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = $"تم رفض الطلب {orderId} بواسطة الادارة",
                    TextEn = $"The Order {orderId} Has Refused by Admin",
                    Type = (int)NotifyTypes.RefusedOrder,
                };
                await _db.NotifyUsers.AddAsync(notifiyUser);
                await _db.SaveChangesAsync();
                var settings = await _db.Settings.Select(x => new
                {
                    AppId = x.ApplicationId,
                    SenderId = x.SenderId
                }).FirstOrDefaultAsync();
                var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.UserId,
                    ProjectName = x.ProjectName
                }).ToList();
                NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                    , order.User.Lang == "ar" ? notifiyUser.TextAr : notifiyUser.TextEn, (int)NotifyTypes.RefusedOrder, orderId, (int)OrderStutes.Refused);
                return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم رفض الطلب بنجاح", "The Order Has been rejected Successfully"));
            }
            return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "حدث خطاء ما", "Something went wrong "));
        }
        public async Task<BaseResponseDto<object>> RefusedOrderByRentalCompany(int orderId, string reasonForCanceled, string lang = "ar")
        {
            var order = await _db.Orders
                //.IgnoreQueryFilters()
                .Include(c => c.RenewCompany)
                .Include(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Include(c => c.DeliverCompany)
                .FirstOrDefaultAsync(c => c.Id == orderId);
            if (order is not null)
            {
                order.OrderStatus = (int)OrderStutes.Refused;
                order.OrderCase = (int)OrderCase.Refused;
                foreach (var deliveryCompany in order.DeliverCompany)
                {
                    deliveryCompany.DeliverCompanyCase = (int)OrderCase.Refused;
                    //deliveryCompany.ReasonRefused = reasonForCanceled;
                }
                order.ReasonToCancled = reasonForCanceled;
                order.RejectionType = (int)RejectionType.RejectionFromRentalCompany;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                var notifiyUser = new NotifyUser
                {
                    OrderId = orderId,
                    UserId = order.UserId,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = $"تم رفض الطلب رقم {orderId} بواسطة شركة التأجير {order.RenewCompany.user_Name}",
                    TextEn = $"The Order {orderId} Has Refused by Rental Company {order.RenewCompany.user_Name}",
                    Type = (int)NotifyTypes.RefusedOrder,
                };
                await _db.NotifyUsers.AddAsync(notifiyUser);
                await _db.SaveChangesAsync();
                var settings = await _db.Settings.Select(x => new
                {
                    AppId = x.ApplicationId,
                    SenderId = x.SenderId
                }).FirstOrDefaultAsync();
                var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.UserId,
                    ProjectName = x.ProjectName
                }).ToList();
                NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                    , order.User.Lang == "ar" ? notifiyUser.TextAr : notifiyUser.TextEn, (int)NotifyTypes.RefusedOrder, orderId, (int)OrderStutes.Refused);
                return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم رفض الطلب بنجاح", "The Order Has been rejected Successfully"));
            }
            return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "حدث خطاء ما", "Something went wrong "));
        }
        public async Task<BaseResponseDto<object>> AcceptOrderByAdministration(int orderId, string lang = "ar")
        {
            var order = await _db.Orders
                //.IgnoreQueryFilters()
                .Include(c => c.RenewCompany)
                .Include(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Include(c => c.DeliverCompany)
                .FirstOrDefaultAsync(c => c.Id == orderId);
            order.OrderCase = (int)OrderCase.WaitToAcceptRentalCompany;
            order.AdminAprovalDate = DateTime.UtcNow.AddHours(3);
            foreach (var deliveryCompany in order.DeliverCompany)
            {
                deliveryCompany.DeliverCompanyCase = (int)OrderCase.WaitToAcceptRentalCompany;
            }
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            // Send Notify To Rental Company And User
            var notifiyUser = new NotifyUser
            {
                OrderId = orderId,
                UserId = order.UserId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم قبول الطلب رقم {orderId} وفي انتظار تأكيد شركة التأجير {order.RenewCompany.user_Name} لاستلام السيارة",
                TextEn = $"The Order number {orderId} has been accepted and is awaiting confirmation from the rental company {order.RenewCompany.user_Name} to pick up the car",
                Type = (int)NotifyTypes.AcceptOrderByAdmin
            };
            var notifyDelgate = new NotifyDelegt
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"لديك طلب جديد برقم {orderId}",
                TextEn = $"You have a new order {orderId}",
                Type = (int)NotifyTypes.NewOrder,
                UserId = order.RentalCompanyId
            };
            await _db.NotifyUsers.AddAsync(notifiyUser);
            await _db.NotifyDelegts.AddAsync(notifyDelgate);
            await _db.SaveChangesAsync();
            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                , order.User.Lang == "ar" ? notifiyUser.TextAr : notifiyUser.TextEn, (int)NotifyTypes.AcceptOrderByAdmin, orderId, (int)OrderStutes.NewOrder);


            var contextIds = await _db.ConnectUser.Where(c => c.UserId == order.RentalCompanyId)
                          .Select(c => c.ContextId)
                          .ToListAsync();

            if (contextIds.Any())
                await _hubContext.Clients.Clients(contextIds).SendAsync("AddNewOrderNotify", new { msg = notifyDelgate.TextAr });

            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم قبول الطلب بنجاح", "The Order was accepted successfully "));
        }
        public async Task<BaseResponseDto<object>> CancelContactByAdministration(int orderId, string lang = "ar")
        {
            var order = await _db.Orders
                .Include(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Include(c => c.OrderCar)
                .Include(c => c.RenewOrderdata)
                .FirstOrDefaultAsync(c => c.Id == orderId);
            double totalPrice = 0;
            int countTakenCarDay = 0;
            double priceForRentalCar = 0;
            if (order.HasRenewToAdditionalPeriod)
            {
                var renewData = order.RenewOrderdata
                    .Where(c => c.Status == (int)OrderStutes.Current)
                    .FirstOrDefault();

                countTakenCarDay = (DateTime.Now - renewData.DateRentalPeriod).Days;
                totalPrice = countTakenCarDay * renewData.DiscountValueForDailyRental;
                priceForRentalCar = renewData.DiscountValueForDailyRental != 0 ? renewData.DiscountValueForDailyRental : order.OrderCar.RentalPriceDaily;

                await _orderClient.CalcateOrderPrice(totalPrice, priceForRentalCar, countTakenCarDay, null, "", renewData);

                renewData.Status = (int)OrderStutes.Finished;
                renewData.Case = (int)OrderCase.CancelContractByAdminAndNotPaidRentalPeriod;
                renewData.CancelContractDate = DateTime.Now;
                _db.Update(renewData);
            }
            else
            {
                countTakenCarDay = (DateTime.Now - order.DateRentalPeriod).Days;
                totalPrice = countTakenCarDay * order.DiscountValueForDailyRental;
                priceForRentalCar = order.DiscountValueForDailyRental != 0 ? order.DiscountValueForDailyRental : order.OrderCar.RentalPriceDaily;
                order.OrderStatus = (int)OrderStutes.Finished;
                order.OrderCase = (int)OrderCase.CancelContractByAdminAndNotPaidRentalPeriod;
                order.DateCancelContact = DateTime.Now;
                _db.Update(order);
            }
            await _db.SaveChangesAsync();
            var notifiyUser = new NotifyUser
            {
                OrderId = orderId,
                UserId = order.UserId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم إلغاء التعاقد{orderId}بواسطه الادارة",
                TextEn = $"The Adminstration Has been Cancel Contract Order{orderId}",
                Type = (int)NotifyTypes.CancelContactByAdmin
            };
            await _db.NotifyUsers.AddAsync(notifiyUser);
            await _db.SaveChangesAsync();
            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
               , order.User.Lang == "ar" ? notifiyUser.TextAr : notifiyUser.TextEn, (int)NotifyTypes.CancelContactByAdmin, orderId, (int)OrderStutes.Finished);
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم الغاء التعاقد بنجاح", "The contract has been successfully cancelled"));
        }
        public async Task<BaseResponseDto<object>> AcceptOrderByRentalCompany(AcceptOrderByRentalCompanyViewModel model)
        {
            var currentDate = HelperDate.GetCurrentDate();
            var order = await _db.Orders
                .Include(c => c.RenewCompany)
                .Include(c => c.OrderCar)
                .Include(c => c.User)
                .ThenInclude(c => c.DeviceId)
                .Include(c => c.DeliverCompany)
                .FirstOrDefaultAsync(c => c.Id == model.OrderId);

            order.OrderCase = (int)OrderCase.WaitToAcceptDeliverCompany;

            order.RentalCompanyAprovalDate = DateTime.UtcNow.AddHours(3);

            order.TimePickupCar = model.TimePickupCar;

            order.DatePickupCar = model.datePickupCar;

            order.LocationpickupCar = model.locationPickupCar;
            order.lat = model.lat;
            order.lng = model.lng;

            if (model.datePickupCar.Date > order.DateRentalPeriod.Date)
            //if (model.datePickupCar > order.DateRentalPeriod || model.datePickupCar < currentDate)
            {
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(model.lang, "من فضلك ادخل  تاريخ مناسب لاستلام السيارة", "Please enter a suitable date to receive the car"));
            }
            foreach (var deliveryCompany in order.DeliverCompany)
            {
                deliveryCompany.DeliverCompanyCase = (int)OrderCase.WaitToAcceptDeliverCompany;
            }
            if (model.CarForm is null)
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(model.lang, "من فضلك ادخل استماره السيارة", "Please Enter Car Form"));

            if (model.FileInsurancInformation is null)
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(model.lang, "من فضلك ادخل ملف تامين السيارة", "Please Enter Car File Insuranc Information"));

            if (model.InsuranceInformation is null)
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(model.lang, "من فضلك ادخل معلومات  تامين السيارة", "Please enter Car insurance information"));

            order.OrderCar.CarForm = await _helper.UploadFileUsingApi(new UploadImageDto { image = model.CarForm, fileName = FileName.Car.ToNumber() });
            order.OrderCar.FileInsurancInformation = await _helper.UploadFileUsingApi(new UploadImageDto { image = model.FileInsurancInformation, fileName = FileName.Car.ToNumber() });
            order.OrderCar.InsuranceInformation = model.InsuranceInformation;
            _db.Orders.Update(order);
            _db.Update(order.OrderCar);
            await _db.SaveChangesAsync();
            var notifiyUser = new NotifyUser
            {
                OrderId = model.OrderId,
                UserId = order.UserId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم قبول طلبك رقم {model.OrderId} بواسطة شركة التأجير  {order.RenewCompany.user_Name}. برجاء التوجه لاستلام سيارتك",
                TextEn = $"Your Order number {model.OrderId} has been accepted by the rental company {order.RenewCompany.user_Name}. Please proceed to pick up your car.",
                Type = (int)NotifyTypes.AcceptOrderByAdmin
            };
            var deliveryCompanysNotifiy = new List<NotifyDelegt>();
            foreach (var item in order.DeliverCompany)
            {
                deliveryCompanysNotifiy.Add(new NotifyDelegt
                {
                    OrderId = model.OrderId,
                    Date = HelperDate.GetCurrentDate(),
                    Show = false,
                    TextAr = $"تم تلقي طلب جديد برقم {model.OrderId}",
                    TextEn = $"A new order has been received with order number {model.OrderId}.",
                    Type = (int)NotifyTypes.NewOrder,
                    UserId = item.DeliverCompanyId
                });
                var contextIds = await _db.ConnectUser.Where(c => c.UserId == order.RentalCompanyId)
                  .Select(c => c.ContextId)
                  .ToListAsync();

                if (contextIds.Any())
                    await _hubContext.Clients.Clients(contextIds).SendAsync("AddNewOrderNotify", new { msg = $"تم تلقي طلب جديد برقم {model.OrderId}" });
            }

            await _db.NotifyUsers.AddAsync(notifiyUser);
            await _db.NotifyDelegts.AddRangeAsync(deliveryCompanysNotifiy);
            await _db.SaveChangesAsync();
            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                , order.User.Lang == "ar" ? notifiyUser.TextAr : notifiyUser.TextEn, (int)NotifyTypes.AcceptOrderByRentalCompany, model.OrderId, (int)OrderStutes.NewOrder);
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تم قبول الطلب بنجاح", "The Order was accepted successfully "));
        }
        public async Task<BaseResponseDto<object>> AcceptOrderByDeliveryCompany(int orderId, string deliveryCompanyId, string lang = "ar")
        {
            var order = await _db.Orders
                 .Include(c => c.User)
                 .ThenInclude(c => c.DeviceId)
                .Include(c => c.DeliverCompany)
                .FirstOrDefaultAsync(c => c.Id == orderId);
            order.OrderCase = (int)OrderCase.Accepted;
            order.OrderStatus = (int)OrderStutes.Current;
            if (order.OrderStatus != (int)OrderStutes.Current)
            {
                order.DeliveryCompanyAprovalDate = DateTime.UtcNow.AddHours(3);
            }
            //  TO Do
            var deliverComapny = order.DeliverCompany.FirstOrDefault(c => c.DeliverCompanyId == deliveryCompanyId);
            deliverComapny.DeliverCompanyCase = (int)OrderCase.Accepted;
            _db.OrderDeliverCompanies.Update(deliverComapny);
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            var notifiyUser = new NotifyUser
            {
                OrderId = orderId,
                UserId = order.UserId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم قبول الطلب {orderId}  {deliverComapny.DeliverCompany.user_Name} بواسطة شركة التوصيل ",
                TextEn = $"The Order {orderId} Has Accpted by Delivery Company {deliverComapny.DeliverCompany.user_Name}",
                Type = (int)NotifyTypes.AcceptOrderByAdmin
            };
            await _db.NotifyUsers.AddAsync(notifiyUser);
            await _db.SaveChangesAsync();
            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                , order.User.Lang == "ar" ? notifiyUser.TextAr : notifiyUser.TextEn, (int)NotifyTypes.AcceptOrderByDeliveryCompany, orderId, (int)OrderStutes.Current);
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم قبول الطلب بنجاح", "The Order was accepted successfully "));
        }
        public async Task<BaseResponseDto<object>> RefusedOrderByDeliveryCompany(int orderId, string deliverCompanyId, string refusedText, string lang = "ar")
        {
            var orderDelivery = await _db.OrderDeliverCompanies
                .Include(c => c.Order)
                .ThenInclude(c => c.User)
                .ThenInclude(c => c.DeviceId)
                 .FirstOrDefaultAsync(c => c.OrderId == orderId && c.DeliverCompanyId == deliverCompanyId);
            orderDelivery.DeliverCompanyCase = (int)OrderCase.Refused;
            orderDelivery.ReasonRefused = refusedText;
            await _db.SaveChangesAsync();

            var checkAllDeliveryRefused = await _db.OrderDeliverCompanies
                .Where(c => c.OrderId == orderId
                  && (c.DeliverCompanyCase == (int)OrderCase.Accepted || c.DeliverCompanyCase == (int)OrderCase.WaitToAcceptDeliverCompany))
                .AnyAsync();

            if (!checkAllDeliveryRefused)
            {
                var order = await _db.Orders.FirstOrDefaultAsync(c => c.Id == orderId);
                order.OrderStatus = (int)OrderStutes.Refused;
                order.OrderCase = (int)OrderCase.Refused;
                order.ReasonToCancled = "";
                order.RejectionType = (int)RejectionType.RejectionFromDeliveryCompany;
                await _db.SaveChangesAsync();
            }
            var notifiyUser = new NotifyUser
            {
                OrderId = orderId,
                UserId = orderDelivery.Order.UserId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم رفض الطلب رقم {orderId} بواسطة شركة التوصيل {orderDelivery.DeliverCompany.user_Name} ",
                TextEn = $"The Order Number {orderId} Has Refused by Delivery Company {orderDelivery.DeliverCompany.user_Name}",
                Type = (int)NotifyTypes.RefusedByDeliveryCompany
            };
            await _db.NotifyUsers.AddAsync(notifiyUser);
            await _db.SaveChangesAsync();
            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            var deviceIds = orderDelivery.Order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                , orderDelivery.Order.User.Lang == "ar" ? notifiyUser.TextAr : notifiyUser.TextEn, (int)NotifyTypes.RefusedByDeliveryCompany, orderId, orderDelivery.Order.OrderStatus);
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم رفض الطلب بنجاح", "The  Order was successfully Refused"));
        }
        public async Task<BaseResponseDto<List<GetRentalCompanyReportViewModel>>> GetReports(string rentalCompanyId, string lang = "ar")
        {
            var cultureInfo = Helper.GetCulture(lang);
            var reports = await _db.Reports
                .Include(c => c.Order.RenewOrderdata)
                .Include(c => c.Order)
                .ThenInclude(c => c.User)
                .Include(c => c.Order.OrderCar)
                .Where(c => c.RentalCompayId == rentalCompanyId)
                .Select(c => new GetRentalCompanyReportViewModel
                {
                    Id = c.Id,
                    CarNumber = c.Order.OrderCar.CarNumber,

                    DailyPriceForRental = c.Order.OrderCar.RentalPriceDaily,

                    DateStartPeriod = c.Order.HasRenewToAdditionalPeriod ?
                    c.Order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().DateRentalPeriod.ToString("dd/MM/yyyy", cultureInfo)
                    : c.Order.DateRentalPeriod.ToString("dd/MM/yyyy", cultureInfo),

                    RentalPeriod = c.Order.HasRenewToAdditionalPeriod
                    ? c.Order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().Duration : c.Order.RentalPeriod,

                    NetPrice = c.Order.HasRenewToAdditionalPeriod
                    ? c.Order.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().NetPrice : c.Order.NetPrice
                }).ToListAsync();
            return ResponseHelper.Success(reports);
        }
        public async Task<BaseResponseDto<ProviderWalletViewModel>> ProviderWallet(string rentalCompanyId)
        {
            var providerWalet = await _db.FinancialAccounts
                  .Where(c => c.RentalCompanyId == rentalCompanyId && c.Confirm == false)
                  .ToListAsync();

            var providerCashOrder = new ProviderFinancialForCashOrderViewModel();
            var providerOnlineOrder = new ProviderFinancialForOnlineOrderViewModel();
            foreach (var cashOrder in providerWalet.Where(c => c.TypeOrderOrderPay == (int)PaymentMethod.Cash))
            {
                providerCashOrder.AppPrice += cashOrder.AppTax;
                providerCashOrder.TotalPrice += cashOrder.TotalOrderPrice;
            }

            foreach (var onlineOrder in providerWalet.Where(c => c.TypeOrderOrderPay == (int)PaymentMethod.Online))
            {
                providerOnlineOrder.AppPrice += onlineOrder.AppTax;
                providerOnlineOrder.TotalPrice += onlineOrder.TotalOrderPrice;
                providerOnlineOrder.ProviderPrice += onlineOrder.ProviderPrice;
            }

            var adminBanks = await _db.AdminBankAccounts.Where(c => c.IsActive).Select(c => new AdminBankViewModel
            {
                Id = c.Id,
                AccountNumber = c.AccountNumber,
                BankAccountName = c.BankAccountName,
                BankName = c.BankName,
                Iban = c.Iban,
            }).ToListAsync();

            return ResponseHelper.Success(new ProviderWalletViewModel
            { AdminBanks = adminBanks, ProviderCashFinancial = providerCashOrder, ProviderOnlineFinancial = providerOnlineOrder });
        }
        public async Task<BaseResponseDto<List<DeliveryCompanyOrdersViewModel>>> GetDeliveryCompanyOrders(int orderStatus, string deliveryCompanyId, string lang = "ar")
        {
            var cultureInfo = Helper.GetCulture(lang);
            var ordersQuery = _db.OrderDeliverCompanies
                 //.IgnoreQueryFilters()
                 .Include(c => c.Order)
                 .ThenInclude(c => c.User)
                 .Where(c => c.DeliverCompanyId == deliveryCompanyId);

            if (orderStatus == (int)OrderStutes.NewOrder)
            {
                ordersQuery = ordersQuery.Where(c =>
                    c.DeliverCompanyCase == (int)OrderCase.WaitToAcceptDeliverCompany && !c.Order.IsCancelContract && !c.Order.IsConfirmReciptCar);
            }
            else
            {
                ordersQuery = ordersQuery.Where(c => c.Order.OrderStatus == orderStatus
                && (c.DeliverCompanyCase != (int)OrderCase.WaitToAcceptDeliverCompany && c.DeliverCompanyCase != (int)OrderCase.Refused));
            }

            var orders = await ordersQuery
                .Select(c => new DeliveryCompanyOrdersViewModel
                {
                    OrderId = c.Order.Id,
                    CaptionName = c.Order.User.user_Name,
                    CaptionPhone = c.Order.User.PhoneCode + " " + c.Order.User.PhoneNumber,
                    CreationDate = c.Order.CreationDate.ToString("dd/MM/yyyy", cultureInfo),
                    OrderType = Helper.GetDeliveryCompanyTypeText(c.TypeDriverInDeliveryCompanies, lang)
                }).OrderByDescending(c => c.OrderId).ToListAsync();
            return ResponseHelper.Success(orders);
        }
        public async Task<BaseResponseDto<List<DeliveryCompanyReportViewModel>>> GetDeliveryCompanyReport(string deliveryCompanyId, string lang = "ar")
        {
            var cultureInfo = Helper.GetCulture(lang);

            var reports = await _db.OrderDeliverCompanies
                 .Include(c => c.Order)
                 .ThenInclude(c => c.User)
                 .Where(c => c.DeliverCompanyId == deliveryCompanyId && c.DeliverCompanyCase == (int)OrderCase.Accepted)
                 .Select(c => new DeliveryCompanyReportViewModel
                 {
                     OrderId = c.OrderId,
                     DriverName = c.Order.User.user_Name,
                     DriverPhone = c.Order.User.PhoneNumber + " " + c.Order.User.PhoneCode + " ",
                     CreationDate = c.Order.CreationDate.ToString("dd/MM/yyyy", cultureInfo),
                     TypeDriverOrder = Helper.GetDeliveryCompanyTypeText(c.TypeDriverInDeliveryCompanies, lang)
                 }).ToListAsync();
            return ResponseHelper.Success(reports);
        }
        public async Task<BaseResponseDto<object>> EnterDataForDeliverApp(EnterDataForDeliveryAppViewModel model)
        {
            await _db.DataForDeliverApps.AddAsync(new DataForDeliverApp
            {
                DeliverCompanyId = model.DeliveryCompantId,
                LoginData = model.LoginData,
                Password = model.Password,
                UserId = model.UserId,
                OrderId = model.OrderId
            });
            await _db.SaveChangesAsync();

            var enterdataForDriver = await _db.DataForDeliverApps
               .Include(c => c.User)
               .ThenInclude(c => c.DeviceId)
               .Include(c => c.DeliverCompany)
               .Where(c => c.OrderId == model.OrderId && c.UserId == model.UserId && c.DeliverCompanyId == model.DeliveryCompantId)
               .FirstOrDefaultAsync();

            var notifiyUser = new NotifyUser
            {
                OrderId = model.OrderId,
                UserId = model.UserId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم اضافة بيانات الدخول علي الطلب رقم{model.OrderId} علي تطبيق شركة التوصيل {enterdataForDriver.DeliverCompany.user_Name}",
                TextEn = $"The login details have been added to order number {model.OrderId} on the delivery company's app {enterdataForDriver.DeliverCompany.user_Name}.",
                Type = (int)NotifyTypes.AcceptOrderByDeliveryCompany
            };

            await _db.NotifyUsers.AddAsync(notifiyUser);
            await _db.SaveChangesAsync();

            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();

            var deviceIds = enterdataForDriver.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();

            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                , enterdataForDriver.User.Lang == "ar" ? notifiyUser.TextAr : notifiyUser.TextEn, (int)NotifyTypes.AcceptOrderByDeliveryCompany, model.OrderId, (int)OrderStutes.Current);

            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تم اضافه البيانات الدخول بنجاح", "Login data has been added successfully"));
        }
        public async Task<BaseResponseDto<object>> ConfirmDataOfDeliveryApp(ComfirmDataOfDeliveryAppViewModel model)
        {
            var orderDeliveruCompany = await _db.OrderDeliverCompanies
                .FirstOrDefaultAsync(c => c.OrderId == model.OrderId && c.DeliverCompanyId == model.DeliveyCompanyId);
            orderDeliveruCompany.ConfirmDataOfDeliveryApp = true;
            await _db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تم تاكيد البيانات بنجاح", "Data Is Confirmed successfully"));
        }

        public async Task<BaseResponseDto<object>> ConfirmRentalCompanyPaidOrder(int orderId, string lang = "ar")
        {
            var order = await _db.Orders
                .Include(c => c.RenewOrderdata)
                .Include(c => c.User)
                 .ThenInclude(c => c.DeviceId)
                .Include(c => c.RenewCompany)
                .Where(c => c.Id == orderId)
                .FirstOrDefaultAsync();
            order.OrderStatus = (int)OrderStutes.Finished;
            order.OrderCase = (int)OrderCase.Finished;
            _db.Update(order);

            var financialAccount = new FinancialAccount();
            if (order.HasRenewToAdditionalPeriod)
            {
                var renewOrderdata = order.RenewOrderdata
                            .OrderByDescending(c => c.Id)
                            .FirstOrDefault();

                renewOrderdata.Status = (int)OrderStutes.Finished;
                renewOrderdata.Case = (int)OrderCase.Finished;

                financialAccount = _orderClient.CreateFinancialAccount(order, renewOrderdata.TypePay, renewOrderdata);


                _db.Update(renewOrderdata);
            }
            else
            {
               

                financialAccount = _orderClient.CreateFinancialAccount(order, order.TypePay);
            }

            await _db.FinancialAccounts.AddAsync(financialAccount);

            await _db.SaveChangesAsync();

            var notifyUser = new NotifyUser
            {
                OrderId = orderId,
                Date = HelperDate.GetCurrentDate(),
                Show = false,
                TextAr = $"تم تاكيد سداد مبلغ التاجير من قبل شركه التاجير {order.RenewCompany.user_Name} وانهاء الطلب بنجاح",
                TextEn = $"The rental payment has been confirmed by the rental company {order.RenewCompany.user_Name}, and the order has been successfully completed.",
                Type = (int)NotifyTypes.EndRentalPeriod,
                UserId = order.UserId
            };

            var settings = await _db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();

            await _db.NotifyUsers.AddAsync(notifyUser);

            await _db.SaveChangesAsync();

            var deviceIds = order.User.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds, null
                  , order.User.Lang == "ar" ? notifyUser.TextAr : notifyUser.TextEn, (int)NotifyTypes.ConfirmReciptCar, orderId, (int)OrderStutes.Current);
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم تاكيد سداد فتره الاستئجار بنجاح", "Successful payment of the rental period has been confirmed"));
        }
        #region Admin Dashboard
        public async Task<BaseResponseDto<List<GetAllOrdersViewModel>>> GetAllOrders(int? orderStatus)
        {
            var lang = Helper.GetLanguage();
            var allOrders = await _db.Orders
                 .Include(c => c.User)
                 .Include(c => c.RenewCompany)
                 .Include(c => c.DeliverCompany)
                 .ThenInclude(c => c.DeliverCompany)
                 .Where(c => orderStatus != null ? c.OrderStatus == orderStatus : true)
                 .Select(c => new GetAllOrdersViewModel
                 {
                     OrderId = c.Id,
                     Date = c.CreationDate.ToString("dd/MM/yyyy hh:mm tt"),


                     DriverName = c.User.user_Name,
                     DriverPhone = c.User.PhoneNumber + " " + c.User.PhoneCode,
                     OrderCaseText = Helper.OrderCaseText(c.OrderCase, lang),
                     OrderStatusText = Helper.StutesText(c.OrderStatus, lang),
                     RentalPeriod = c.RentalPeriod,
                     TotalPrice = c.NetPrice,
                     RentalCompany = c.RenewCompany.user_Name,
                     DliveryCompanys = c.DeliverCompany.Select(c => c.DeliverCompany.user_Name).ToList(),

                 }).OrderByDescending(c => c.OrderId).ToListAsync();
            return ResponseHelper.Success(allOrders);
        }
        public async Task<BaseResponseDto<List<GetRentalCompanyOrdersByAdminViewModel>>> GetRentalCompanyOrderByAdmin(int? filterType)
        {
            var currentDate = HelperDate.GetCurrentDate();
            var lang = Helper.GetLanguage();
            var query = _db.Orders
                 .Include(c => c.RenewOrderdata)
                 .Include(c => c.RenewCompany)
                 .Include(c => c.User)
                 .Include(c => c.OrderCar)
                  .Where(c => !c.User.IsDeleted);

            if (filterType == (int)RentalCompanyOrder.newOdrer)
                query = query.Where(c => c.OrderStatus == (int)OrderStutes.NewOrder);

            if (filterType == (int)RentalCompanyOrder.Current)
                query = query.Where(c => c.OrderStatus == (int)OrderStutes.Current && !c.HasRenewToAdditionalPeriod);

            if (filterType == (int)RentalCompanyOrder.RenewOrder)
                query = query.Where(c => c.HasRenewToAdditionalPeriod
                        && c.OrderStatus == (int)OrderStutes.Current && !c.IsCancelContract);

            if (filterType == (int)RentalCompanyOrder.CancelContractOrder)
                query = query.Where(c => c.IsCancelContract);

            if (filterType == (int)RentalCompanyOrder.Finished)
                query = query.Where(c => c.OrderStatus == (int)OrderStutes.Finished && !c.IsCancelContract);


            var orders = await query.Select(c => new GetRentalCompanyOrdersByAdminViewModel
            {
                OrderId = c.Id,
                Date = c.CreationDate.ToString("dd/MM/yyyy"),
                NetPrice = c.NetPrice,
                RentalCompany = c.RenewCompany.user_Name,
                CarNumber = c.OrderCar.CarNumber,
                carmodel = _db.CarModels.FirstOrDefault(m => m.Id == c.OrderCar.CarModelId).NameAr,
                carcategory = _db.CarCategories.FirstOrDefault(m => m.Id == c.OrderCar.CarCategoryId).NameAr,
                cartype = _db.CarCategories.FirstOrDefault(m => m.Id == c.OrderCar.CarTypeId).NameAr,
                DailyRentalForCar = c.OrderCar.RentalPriceDaily,
                DriverLiscenceImage = DashBordUrl.baseUrlHost + c.DrivingLicenseImage,
                CarForm = c.OrderCar.CarForm != null ? DashBordUrl.baseUrlHost + c.OrderCar.CarForm : "",
                DriverName = c.User.user_Name,
                DriverPhone = c.User.PhoneCode + " " + c.User.PhoneNumber,
                RentalPeriod = c.RentalPeriod,
                OrderCase = c.OrderCase,
                OrderStatus = c.OrderStatus,
                OrderStatusText = Helper.StutesText(c.OrderStatus, lang),
                OrderCaseText = Helper.OrderCaseText(c.OrderCase, lang),
                RemainPeriod = (c.DateEndRentalPeriod - currentDate).Days,
                RenewPeriod = c.RenewOrderdata != null && c.RenewOrderdata.Any(r => r.Status == (int)OrderStutes.Current) ?
                c.RenewOrderdata.FirstOrDefault(c => c.Status == (int)OrderStutes.Current).Duration : 0,
                //RenewPrice=c.RenewOrderdata != null || c.RenewOrderdata.Count() > 0 ?
                //c.RenewOrderdata.FirstOrDefault(c => c.Status == (int)OrderStutes.Current).NetPrice : 0
            }).OrderByDescending(c => c.OrderId).ToListAsync();
            return ResponseHelper.Success(orders);
        }
        public async Task<BaseResponseDto<List<GetDeliveryCompanyOrdersByAdminViewModel>>> GetDeliveyCompanyOrdersByAdmin(int? ordercase)
        {
            var orders = await _db.OrderDeliverCompanies
                 .Include(c => c.DeliverCompany)
                 .Include(c => c.Order)
                 .Include(c => c.Order.User)
                 .Include(c => c.Order.OrderCar)
                 .Where(c => (ordercase != null ? c.DeliverCompanyCase == ordercase : true))
                 .Select(c => new GetDeliveryCompanyOrdersByAdminViewModel
                 {
                     OrderDeliveryId = c.Id,
                     OrderId = c.OrderId,
                     DeliveryCompanyName = c.DeliverCompany.user_Name,
                     Date = c.Order.CreationDate.ToString("dd/MM/yyyy"),
                     RentalPeriod = c.Order.RentalPeriod,
                     DriverName = c.Order.User.user_Name,
                     DriverPhone = c.Order.User.PhoneNumber + "" + c.Order.User.PhoneCode,
                     DriverLiscenceImage = DashBordUrl.baseUrlHost + c.Order.DrivingLicenseImage,
                     CarNumber = c.Order.OrderCar.CarNumber,
                     carmodel = _db.CarModels.FirstOrDefault(m => m.Id == c.Order.OrderCar.CarModelId).NameAr,
                     carcategory = _db.CarCategories.FirstOrDefault(m => m.Id == c.Order.OrderCar.CarCategoryId).NameAr,
                     cartype = _db.CarCategories.FirstOrDefault(m => m.Id == c.Order.OrderCar.CarTypeId).NameAr,
                     CarForm = c.Order.OrderCar.CarForm != null ? DashBordUrl.baseUrlHost + c.Order.OrderCar.CarForm:"",
                 }).OrderByDescending(c => c.OrderId)
                 .ToListAsync();
            return ResponseHelper.Success(orders);
        }
        public async Task<BaseResponseDto<OrderDetailsByAdminViewModel>> GetOrderDetailsByAdmin(int orderId)
        {
            var orderDetails = await _db.Orders
                  .Include(c => c.RenewOrderdata)
                  .Include(c => c.User)
                  .Include(c => c.RenewCompany)
                  .Include(c => c.DeliverCompany)
                  .ThenInclude(c => c.DeliverCompany)
                  .Where(c => c.Id == orderId).Select(c => new OrderDetailsByAdminViewModel
                  {
                      OrderId = c.Id,

                      Date = c.CreationDate.ToString("dd/MM/yyyy"),

                      DateRentalPeriod = c.DateRentalPeriod.ToString("dd/MM/yyyy"),

                      DriverName = c.User.user_Name,

                      DriverPhone = c.User.PhoneNumber + " " + c.User.PhoneCode,

                      DriverLicenseImage = DashBordUrl.baseUrlHost + c.DrivingLicenseImage,

                      IdentityImage = DashBordUrl.baseUrlHost + c.IdentityImage,

                      IdentityNumber = c.IdentityNumber,

                      OrderCaseText = Helper.OrderCaseText(c.OrderCase, "ar"),

                      OrderCase = c.OrderCase,
                      OrderStatus = c.OrderStatus,
                      RentalPeriod = c.RentalPeriod,

                      RenewRetalPeriod = c.RenewOrderdata != null && c.RenewOrderdata.Any() ?
                           c.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().Duration : 0,

                      RenewOrderPrice = c.RenewOrderdata != null && c.RenewOrderdata.Any() ?
                           c.RenewOrderdata.OrderByDescending(c => c.Id).Select(c => c.NetPrice + c.BreakPeriodPrice + c.RentalConfirmationDelayPrice).FirstOrDefault() : 0,

                      BreakPeriodForRenew = c.RenewOrderdata != null && c.RenewOrderdata.Any() ?
                       c.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().BreakPeriod : 0,



                      BreakPriceForRenew = c.RenewOrderdata != null && c.RenewOrderdata.Any() ?
                       c.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().BreakPeriodPrice : 0,

                      RentalDailyPeriodForRenew = c.RenewOrderdata != null && c.RenewOrderdata.Any() ?
                       c.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().RentalConfirmationDelayPeriod : 0,

                      RentailDailyPriceForRenew = c.RenewOrderdata != null && c.RenewOrderdata.Any() ?
                       c.RenewOrderdata.OrderByDescending(c => c.Id).FirstOrDefault().RentalConfirmationDelayPrice : 0,


                      RentalCompany = c.RenewCompany.user_Name,

                      DeliveyCompanies = c.DeliverCompany
                          .Where(c => c.DeliverCompanyCase != (int)OrderCase.Refused)
                          .Select(c => c.DeliverCompany.user_Name).ToList(),

                      CoponCode = c.CoponCode ?? "",
                      NetPrice = c.NetPrice + c.BreakPeriodPrice + c.RentalConfirmationDelayPrice,
                      SubPrice = c.subTotal,
                      DiscountValue = c.subTotal - c.NetPrice,

                      BreakPeriodForOrder = c.BreakPeriod,
                      BreakPriceForOrder = c.BreakPeriodPrice,

                      RentalDailyPeriodForOrder = c.RentalConfirmationDelayPeriod,
                      RentailDailyPriceForOrder = c.RentalConfirmationDelayPrice,

                      CarNumber = c.OrderCar.CarNumber,
                      CarModel = _db.CarModels.FirstOrDefault(m => m.Id == c.OrderCar.CarModelId).NameAr,
                      CarCategory = _db.CarCategories.FirstOrDefault(m => m.Id == c.OrderCar.CarCategoryId).NameAr,
                      CarType = _db.CarTypes.FirstOrDefault(m => m.Id == c.OrderCar.CarTypeId).NameAr,
                      CarForm = c.OrderCar.CarForm != null ? DashBordUrl.baseUrlHost + c.OrderCar.CarForm : "",
                  }).FirstOrDefaultAsync();
            return ResponseHelper.Success(orderDetails);
        }
        public async Task<BaseResponseDto<List<OrderCashFinancialAccountIndexViewModel>>> OrderCashConfirmBlancedRequest(bool confirm, int typepayOrder)
        {
            var providerConfirmRequest = await _db.BankTransfers
                .Include(c => c.Provider)
                .Include(c => c.AdminBank)
                .Where(c => c.Confirm == confirm && c.TypeOrderOrderPay == typepayOrder)
                .Select(c => new OrderCashFinancialAccountIndexViewModel
                {
                    Id = c.Id,
                    ProviderEmail = c.Provider.Email,
                    ProviderName = c.Provider.user_Name,
                    ProviderPhone = c.Provider.PhoneNumber + " " + c.Provider.PhoneCode,
                    AdminBankBankIBN = c.AdminBank.Iban,
                    AdminBankBankName = c.AdminBank.BankName,
                    TotalOrderPrice = c.Total,
                    AppTax = c.AppTax,
                    ProviderPrice = c.Total - c.AppTax,

                }).ToListAsync();
            return ResponseHelper.Success(providerConfirmRequest);
        }
        public async Task<BaseResponseDto<List<OrderOnlineFinancialAccountIndexViewModel>>> OrderOnlineConfirmBlancedRequest(bool confirm, int typepayOrder)
        {
            var providerConfirmRequest = await _db.BankTransfers
                .Include(c => c.Provider)
                .Where(c => c.Confirm == confirm && c.TypeOrderOrderPay == typepayOrder)
                .Select(c => new OrderOnlineFinancialAccountIndexViewModel
                {
                    Id = c.Id,
                    ProviderEmail = c.Provider.Email,
                    ProviderName = c.Provider.user_Name,
                    ProviderPhone = c.Provider.PhoneNumber + " " + c.Provider.PhoneCode,
                    ProviderBankIBN = c.Iban,
                    ProviderBankName = c.BankName,
                    TotalOrderPrice = c.Total,
                    AppTax = c.AppTax,
                    ProviderPrice = c.Total - c.AppTax,

                }).ToListAsync();
            return ResponseHelper.Success(providerConfirmRequest);
        }
        public async Task<BaseResponseDto<object>> ConfirmProviderbalance(int id, string lang = "ar")
        {

            var bankTransfer = await _db.BankTransfers
            .FirstOrDefaultAsync(c => c.Id == id);
            bankTransfer.Confirm = true;
            await _db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم تسويه الحساب بنجاح", "The account has been settled successfully"));
        }
        #endregion
        public async Task<BaseResponseDto<object>> RequestToConfirmMySettle(RequestToConfirmMysettleViewModel model)
        {
            var orders = await _db.FinancialAccounts
            .Where(c => c.RentalCompanyId == model.CompanyId
                && c.Confirm == false
                && c.TypeOrderOrderPay == (int)PaymentMethod.Online)
            .ToListAsync();
            var banktransfer = new BankTransfer();
            banktransfer.ProviderId = model.CompanyId;
            banktransfer.Total = orders.Sum(c => c.TotalOrderPrice);
            banktransfer.AppTax = orders.Sum(c => c.AppTax);
            banktransfer.Iban = model.Iban;
            banktransfer.BankName = model.BankName;
            banktransfer.AccountNumber = model.AccountNumber;
            banktransfer.AccountOwnerName = model.AccountOwnerName;
            banktransfer.Confirm = false;
            banktransfer.TypeOrderOrderPay = (int)PaymentMethod.Online;
            await _db.BankTransfers.AddAsync(banktransfer);
            foreach (var item in orders)
            {
                item.Confirm = true;
            }
            _db.FinancialAccounts.UpdateRange(orders);
            await _db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تم ارسال طلب التسوية بنجاح", "The settlement request has been sent successfully"));
        }
        public async Task<BaseResponseDto<object>> RequestToConfirmAdminSettle(RequestToConfirmAdminSettleViewModel model)
        {
            var orders = await _db.FinancialAccounts
                .Where(c => c.RentalCompanyId == model.CompanyId
                    && c.Confirm == false
                    && c.TypeOrderOrderPay == (int)PaymentMethod.Cash)
                     .ToListAsync();
            var banktransfer = new BankTransfer();
            banktransfer.ProviderId = model.CompanyId;
            banktransfer.Total = orders.Sum(c => c.TotalOrderPrice);
            banktransfer.AppTax = orders.Sum(c => c.AppTax);
            banktransfer.Confirm = false;
            banktransfer.AdminBankId = model.AdminBankId;
            banktransfer.TypeOrderOrderPay = (int)PaymentMethod.Cash;
            await _db.BankTransfers.AddAsync(banktransfer);
            foreach (var item in orders)
            {
                item.Confirm = true;
            }
            _db.FinancialAccounts.UpdateRange(orders);
            await _db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تم ارسال طلب التسوية بنجاح", "The settlement request has been sent successfully"));
        }

    }
}
