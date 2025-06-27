using AAITHelper.ModelHelper;
using AAITHelper;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAITHelper.Enums;
using AutoMapper.QueryableExtensions;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mapster;
using Mumaken.Domain.DTO;
using Azure.Core;
using Mumaken.Domain.Common.Helpers.ResponseHelper;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.ViewModel.Provider;
using Mumaken.Domain.DTO.SettingDto;
using Org.BouncyCastle.Crypto.Tls;

namespace Mumaken.Service.Api.Implementation.Auth
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHelper _helper;

        public UserServices(ApplicationDbContext db, UserManager<ApplicationDbUser> userManager, ICurrentUserService currentUserService, IHelper uploadImage = null)
        {
            this.db = db;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _helper = uploadImage;
        }
        #region User
        public async Task<BaseResponseDto<UserInfoDTO>> GetUserInfo(string userId, string lang, string token)
        {
            var currentDate = DateTime.UtcNow.AddHours(3);
            var user = await db.Users
                .Include(c => c.city)
                .FirstOrDefaultAsync(x => x.Id == userId && x.TypeUser == UserType.Client.ToNumber());

            var userInfo = user.Adapt<UserInfoDTO>();
            userInfo.token = token;
            userInfo.age = currentDate.Year - user.BirthDate.Year;
            var permissiblePeriod = await db.Settings
                            .Select(c => c.PermissiblePeriod)
                            .FirstOrDefaultAsync();

            userInfo.permissiblePeriod= permissiblePeriod;
            userInfo.birthDate = user.BirthDate.ToString("yyyy/MM/dd");
            return ResponseHelper.Success(userInfo);
        }
        public async Task<BaseResponseDto<UserInfoDTO>> UpdateDataUser(UpdateDataUserDto userModel)
        {
            var user = (await db.Users.Where(x => x.Id == _currentUserService.UserId).FirstOrDefaultAsync());

            user.user_Name = userModel.userName ?? user.user_Name;
            user.CityId = userModel.cityId ?? user.CityId;
            user.BirthDate = userModel.birhDate;
            if (userModel.imgProfile != null)
            {
                user.ImgProfile = _helper.Upload(userModel.imgProfile, (int)FileName.Users);
            }
            await db.SaveChangesAsync();
            var userInfo = await GetUserInfo(user.Id, userModel.lang, _helper.GenerateToken(user));
            return ResponseHelper.Success(userInfo.Data, HelperMsg.MsgValidation(EnumValidMsg.Auth.UpdateSuccessfully.ToNumber(), userModel.lang));

        }
        public async Task<BaseResponseDto<object>> GetUserWalet()
        {
            var currentUser = await db.Users
                .FirstOrDefaultAsync(c => c.Id == _currentUserService.UserId);
            return ResponseHelper.Success<object>(currentUser.Walet);
        }
        #endregion
        #region Provider
        public async Task<ProviderInfoDto> GetProviderInfo(string userId, string lang, string token)
        {
            var user = await db.Users
                .Include(c => c.city)
                .Include(c => c.Distract)
                .Where(x => x.Id == userId)
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync();
            var infoProvider = new ProviderInfoDto
            {
                id = user.Id,
                imgProfile = DashBordUrl.baseUrlHost + user.ImgProfile,
                userName = user.user_Name,
                phone = user.PhoneNumber,
                phoneCode = user.PhoneCode,
                typeUser = user.TypeUser,
                email = user.Email,
                code = user.Code,
                ActiveCode = user.ActiveCode,
                closeNotify = user.CloseNotify,
                cityId = user.CityId,
                city = user.city?.ChangeLangName(user.Lang), // Using null conditional operator to avoid null reference exception
                distractName = user.Distract?.ChangeLangName(user.Lang), // Using null conditional operator
                distractId = user.DistrictId.Value,
                commercialRegisterNumber = user.CommercialRegisterNumber,
                location = user.Location,
                lat = user.Lat,
                lng = user.Lng,
                lang = user.Lang,
                token = token,
            };

            return infoProvider;
        }
        public async Task<BaseResponseDto<ProviderInfoDto>> UpdateProvider(UpadateProviderViewModel model)
        {
            var lang = Helper.GetLanguage();
            var provider = await db.Users.FirstOrDefaultAsync(c => c.Id == model.id);
            model.lang = lang;
            provider.user_Name = model.userName;
            provider.CityId = model.cityId;
            provider.DistrictId = model.distractId;
            provider.Location = model.address;
            provider.Lat = model.lat;
            provider.Lng = model.lng;
            provider.CommercialRegisterNumber = model.commercialRegisterNumber;
            if (model.imageProfile != null)
            {
                provider.ImgProfile = await _helper.UploadFileUsingApi(new UploadImageDto { image = model.imageProfile, fileName = (int)FileName.Users });
            }
            await db.SaveChangesAsync();
            var providerInfo = await GetProviderInfo(provider.Id, model.lang, "");
            return ResponseHelper.Success(providerInfo, HelperMsg.MsgValidation(EnumValidMsg.Auth.UpdateSuccessfully.ToNumber(), model.lang));
        }
        public async Task<BaseResponseDto<object>> DeleteAllDelgateNotify(string providerId, string lang = "ar")
        {
            var notifyDelgate = await db.NotifyDelegts
                .Where(c => c.UserId == providerId)
                .ToListAsync();
            db.NotifyDelegts.RemoveRange(notifyDelgate);
            await db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم الحذف بنجاح", "Deleted successfully"));
        }
        public async Task<BaseResponseDto<object>> DeleteDelgateNotifyById(int notifyId, string lang = "ar")
        {
            var providerNotification = await db.NotifyDelegts.FirstOrDefaultAsync(n => n.Id == notifyId);
            db.NotifyDelegts.Remove(providerNotification);
            await db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(lang, "تم الحذف بنجاح", "Deleted successfully"));
        }
        public async Task<BaseResponseDto<List<NotifyListDto>>> ListOfNotifyProvider(string userId, string lang = "ar")
        {
            List<NotifyListDto> Notify = await db.NotifyDelegts
                .Where(x => x.UserId == userId)
                .Select(x => new NotifyListDto
                {
                    id = x.Id,
                    text = HelperMsg.creatMessage(lang, x.TextAr, x.TextEn),
                    date = x.Date.ToString("dd/MM/yyyy h:mm tt"),
                    type = x.Type,
                    orderId=x.OrderId

                }).OrderByDescending(x => x.id).ToListAsync();
            var updateNotfy = await db.NotifyDelegts.Where(x => x.Show == false && x.UserId == userId).ToListAsync();
            updateNotfy.ForEach(a => a.Show = true);
            await db.SaveChangesAsync();
            return ResponseHelper.Success(Notify);
        }
        #endregion
        #region Admin Dashboard
        public async Task<BaseResponseDto<object>> ChangeState(string id)
        {
            var user = await db.Users
                .Include(c => c.DeviceId)
                .FirstOrDefaultAsync(c => c.Id==id);

            user.IsActive = !user.IsActive;
            await db.SaveChangesAsync();

            var settings = await db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            var deviceIds = user.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            if (user.IsActive == false)
            {
                NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds,
                    null, user.Lang == "ar" ? "تم تسجيل الخروج من فضلك تواصل مع الادارة" : "Signed out, please contact the admin",
                    (int)NotifyTypes.BlockUser);
            }
            db.RemoveRange(user.DeviceId);
            await db.SaveChangesAsync();
            //else
            //{
            //    NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds,
            //       null, user.Lang == "ar" ? "تم تفعيل حسابك من قبل الاداره" : "Your Account Has Been Active By Admin",
            //       (int)NotifyTypes.BlockUser);
            //}
            return ResponseHelper.Success<object>(user.IsActive);
        }
        public async Task<BaseResponseDto<object>> DeleteAccount(string id, string lang = "ar")
        {
            var checkHasOrder = db.Orders.Where(c => (c.UserId == id || c.RentalCompanyId == id)
                && (c.OrderStatus == (int)OrderStutes.Current && c.OrderStatus == (int)OrderStutes.NewOrder))
                .Any();
            if (checkHasOrder)
            {
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "لا يمكنك حذف مستخدم لديه طلبات جديده او طلبات حاليه", 
                    "You cannot delete a user who has new orders or existing orders"));
            }
            var deletedUser = await db.Users
                .Include(c=>c.DeviceId)
                .FirstOrDefaultAsync(c => c.Id == id);

            deletedUser.user_Name = deletedUser.user_Name + Guid.NewGuid().ToString();
            deletedUser.PhoneNumber = deletedUser.PhoneNumber + Guid.NewGuid().ToString();
            deletedUser.Email = deletedUser.Email + Guid.NewGuid().ToString();
            deletedUser.NormalizedEmail = deletedUser.NormalizedEmail + Guid.NewGuid().ToString();
            deletedUser.UserName = deletedUser.UserName + Guid.NewGuid().ToString();
            deletedUser.NormalizedUserName = deletedUser.NormalizedUserName + Guid.NewGuid().ToString();
            deletedUser.IsDeleted = true;
            await db.SaveChangesAsync();
            var settings = await db.Settings.Select(x => new
            {
                AppId = x.ApplicationId,
                SenderId = x.SenderId
            }).FirstOrDefaultAsync();
            var deviceIds = deletedUser.DeviceId.Select(x => new DeviceIdModel
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName
            }).ToList();
            NotificationHelper.SendPushNotification(settings.AppId, settings.SenderId, deviceIds,
                 null, deletedUser.Lang == "ar" ? "تم حذف الحساب برجاء التواصل مع الادارة" : "The account has been deleted. Please contact the administration",
                 (int)NotifyTypes.BlockUser);
            return ResponseHelper.Success<object>(true,HelperMsg.creatMessage(lang,"تم الحذف بنجاح", "Deleted successfully"));
        }
        public async Task<BaseResponseDto<object>> AcceptRequestJoin(string id,string lang = "ar")
        {
            var user=await db.Users.FirstOrDefaultAsync(c => c.Id == id);
            user.IsAppravel=true;
            await db.SaveChangesAsync();
            return ResponseHelper.Success<object>(user.IsAppravel,
                HelperMsg.creatMessage(lang, "تم قبول طلب الانضمام بنجاح", "Your request to join has been successfully accepted"));
        }
        #endregion

        #region Helper Function
        public async Task<bool> CheckValidateEmail(string email)
        {

            ApplicationDbUser foundedEmail = await _userManager.FindByEmailAsync(email);
            return foundedEmail != null;
        }
        public async Task<bool> CheckValidatePhone(string phone, string phoneCode)
        {
            ApplicationDbUser foundedPhone = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phone && x.PhoneCode == phoneCode);
            return foundedPhone != null;
        }
        #endregion

        public async Task<BaseResponseDto<object>> DeleteAllNotify(string lang = "ar")
        {
            var currentUserId = _currentUserService.UserId;
            var userNotifications = db.NotifyUsers.Where(n => n.UserId == currentUserId);
            db.NotifyUsers.RemoveRange(userNotifications);
            await db.SaveChangesAsync();
            return ResponseHelper.Success<object>(HelperMsg.creatMessage(lang, "تم الحذف بنجاح", "Deleted successfully"));
        }
        public async Task<BaseResponseDto<object>> DeleteNotifyById(int notifyId, string lang = "ar")
        {
            var currentUserId = _currentUserService.UserId;
            var userNotification = await db.NotifyUsers.FirstOrDefaultAsync(n => n.Id == notifyId);
            db.NotifyUsers.Remove(userNotification);
            await db.SaveChangesAsync();
            return ResponseHelper.Success<object>(HelperMsg.creatMessage(lang, "تم الحذف بنجاح", "Deleted successfully"));
        }
        public async Task<BaseResponseDto<List<NotifyListDto>>> ListOfNotifyUser(string userId, string lang = "ar")
        {
            List<NotifyListDto> Notify = await db.NotifyUsers.Where(x => x.UserId == userId).Select(x => new NotifyListDto
            {
                id = x.Id,
                text = HelperMsg.creatMessage(lang, x.TextAr, x.TextEn),
                date = x.Date.ToString("dd/MM/yyyy h:mm tt"),
                type = x.Type,
                orderId=x.OrderId

            }).OrderByDescending(x => x.id).ToListAsync();
            List<NotifyUser> updateNotfy = await db.NotifyUsers.Where(x => x.Show == false && x.UserId == userId).ToListAsync();
            updateNotfy.ForEach(a => a.Show = true);
            await db.SaveChangesAsync();
            return ResponseHelper.Success(Notify);
        }

        public async Task<bool> ChangeNotify(ChangeNotifyEditDto changeNotifyEditDto)
        {
            ApplicationDbUser foundeUser = await db.Users.FindAsync(changeNotifyEditDto.userId);
            foundeUser.CloseNotify = changeNotifyEditDto.notify;
            await db.SaveChangesAsync();
            return true;

        }
        public async Task<bool> AddUserNotify(string textAr, string textEn, string userId, int stutes, int orderId = 0)
        {
            try
            {
                await db.NotifyUsers.AddAsync(new()
                {
                    Date = HelperDate.GetCurrentDate(),
                    UserId = userId,
                    Show = false,
                    TextAr = textAr,
                    TextEn = textEn,
                    Type = stutes,
                    OrderId = orderId

                });
                await db.HistoryNotify.AddAsync(new()
                {
                    Text = textAr,
                    Date = HelperDate.GetCurrentDate(),
                });
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> SendNotifyAsync(string textAr, string textEn, string userId, int stutes)
        {
            try
            {
                var user = await db.Users.FindAsync(userId);

                await db.NotifyUsers.AddAsync(new()
                {
                    Date = HelperDate.GetCurrentDate(),
                    UserId = userId,
                    Show = false,
                    TextAr = textAr,
                    TextEn = textEn,
                    Type = stutes
                });
                await db.SaveChangesAsync();

                var setting = await db.Settings.FirstOrDefaultAsync();

                List<DeviceIdModel> AllDeviceids = await db.DeviceIds.Where(x => x.UserId == userId).Select(x => new DeviceIdModel()
                {
                    DeviceId = x.DeviceId_,
                    DeviceType = x.DeviceType,
                    FkUser = x.UserId,
                    ProjectName = x.ProjectName

                }).ToListAsync();
                HelperFcm.SendPushNotification(setting.ApplicationId, setting.SenderId, AllDeviceids, null, textAr, stutes);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> SendNotify(string textAr, string textEn, string fkProvider, int stutes, int orderId = 0)
        {
            ApplicationDbUser user = await _userManager.FindByIdAsync(fkProvider);
            Setting Fcm = await db.Settings.FirstOrDefaultAsync();
            List<DeviceIdModel> AllDeviceids = await db.DeviceIds.Where(x => x.UserId == fkProvider).Select(x => new DeviceIdModel()
            {
                DeviceId = x.DeviceId_,
                DeviceType = x.DeviceType,
                FkUser = x.UserId,
                ProjectName = x.ProjectName

            }).AsNoTracking().ToListAsync();
            HelperFcm.SendPushNotification(Fcm.ApplicationId, Fcm.SenderId, AllDeviceids, null, textAr, stutes, user.TypeUser, orderId);
            return true;
        }





    }
}
