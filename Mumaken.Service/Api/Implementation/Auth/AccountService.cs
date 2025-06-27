using AAITHelper;
using AAITHelper.Enums;
using AAITHelper.ModelHelper;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.DTO.CategoryDto;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mumaken.Domain.DTO;
using Mapster;
using Mumaken.Domain.Common.Helpers.ResponseHelper;
using NPOI.HSSF.Record;
using NPOI.OpenXmlFormats.Wordprocessing;
using Newtonsoft.Json.Linq;
using Mumaken.Domain.ViewModel.Provider;
using Mumaken.Service.Mapping;
using Mumaken.Domain.ViewModel.Auth;
using Org.BouncyCastle.Crypto.Tls;
using RestSharp;
using Mumaken.Domain.DTO.SettingDto;
using Mumaken.Domain.Common.Helpers._4jawaly.Models._4jawalyProvider;

namespace Mumaken.Service.Api.Implementation.Auth
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHelper _helper;
        private readonly IUserServices _userService;
        public AccountService(ApplicationDbContext db, UserManager<ApplicationDbUser> userManager, ICurrentUserService currentUserService, IHelper helper, IUserServices userService)
        {
            this.db = db;
            _userManager = userManager;
            _currentUserService = currentUserService;
            _helper = helper;
            _userService = userService;
        }

        public async Task<bool> AddDeviceId(DeviceIdAddDto deviceIdAddDto)
        {
            try
            {
                var check_device_id = await db.DeviceIds
                     .Where(c => c.DeviceId_ == deviceIdAddDto.deviceId && c.UserId == deviceIdAddDto.userId)
                     .AnyAsync();

                if (!check_device_id)
                {
                    var deviceId = new DeviceId
                    {
                        DeviceId_ = deviceIdAddDto.deviceId,
                        UserId = deviceIdAddDto.userId,
                        DeviceType = deviceIdAddDto.deviceType,
                        ProjectName = "Mumaken",
                        Date = DateTime.UtcNow.AddHours(3)
                    };
                    await db.DeviceIds.AddAsync(deviceId);
                    await db.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #region User
        public async Task<BaseResponseDto<UserInfoDTO>> AddUser(UserAddDTO model)
        {
            var user = model.Adapt<ApplicationDbUser>();
            string imageProfile = "";
            if (model.imageProfile is not null)
                imageProfile = _helper.Upload(model.imageProfile, (int)FileName.Users);
            else
                imageProfile = "images/Users/default.jpg";

            user.ImgProfile = imageProfile;

            #region validation
            bool userWithSamePhoneNumber = await _userService.CheckValidatePhone(model.phone, model.phoneCode);
            if (userWithSamePhoneNumber)
                return ResponseHelper.Failure<UserInfoDTO>(HelperMsg.creatMessage(model.lang,"رقم الجوال مسجل مسبقا", "Mobile number is already registered"));

            bool userWithSameEmail = await _userService.CheckValidateEmail(model.email);
            if (userWithSameEmail)
                return ResponseHelper.Failure<UserInfoDTO>(HelperMsg.MsgValidation(EnumValidMsg.Auth.EmailExisting.ToNumber(), model.lang));

            #endregion
            int code = await GenerateCode(1234);
            user.Code = code;
            var result = await _userManager.CreateAsync(user, user.ShowPassword);
            if (result.Succeeded)
            {
                //var token = _helper.GenerateToken(user);

                var userInfo = await _userService.GetUserInfo(user.Id, model.lang, "");

                var deviceModel = model.Adapt<DeviceIdAddDto>();
                deviceModel.userId = user.Id;
                await AddDeviceId(deviceModel);
                _ = await SendSms(code, user.PhoneNumber);
                return ResponseHelper.Success(userInfo.Data, HelperMsg.MsgValidation(EnumValidMsg.Auth.RegisterSuccessfully.ToNumber(), model.lang));
            }
            return ResponseHelper.Failure<UserInfoDTO>(HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), model.lang));
        }
        public async Task<BaseResponseDto<ForgetPasswordDto>> ForgetUserPassword(ForgetPasswordAddDto model)
        {
            model.lang = model.lang ?? "ar";
            var user = await GetUserByEmail(model.email);
            if (user is null)

                return ResponseHelper.Failure<ForgetPasswordDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.UserNotFound.ToNumber(), model.lang));

            if (!(await CheckAccountBlocked(user)))
                return ResponseHelper.Failure<ForgetPasswordDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.Accountblocked.ToNumber(), model.lang));

            int code = await GenerateCode(1234);
            user.Code = code;
            await db.SaveChangesAsync();
            // send Code With Email
            _ = await SendSms(code, user.PhoneNumber);
            var data = new ForgetPasswordDto { code = code, userId = user.Id };
            return ResponseHelper.Success(data);
        }
        public async Task<BaseResponseDto<ChangeEmailResponseDto>> ChangeEmail(ChangeEmailDto model)
        {
            var user = await GetCurrentUser();
            var passwordCheck = await CheckPassword(user, model.password, model.lang);

            if (!passwordCheck.IsSuccess)
                return ResponseHelper.Failure<ChangeEmailResponseDto>(passwordCheck.Message);

            bool validateNewEmail = await _userService.CheckValidateEmail(model.email);
            if (validateNewEmail)
                return ResponseHelper.Failure<ChangeEmailResponseDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.EmailExisting.ToNumber(), model.lang));


            int code = await GenerateCode(1234);
            user.Code = code;
            await db.SaveChangesAsync();
            // Send Code To Email
            _ = SendSms(code, user.PhoneNumber);

            var data = new ChangeEmailResponseDto { code = user.Code, newEmail = model.email, userId = user.Id };

            return ResponseHelper.Success(data, HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeSent.ToNumber(), model.lang));
        }
        public async Task<BaseResponseDto<UserInfoDTO>> ConfirmChangeEmail(ConfirmChangeEmailDto model)
        {
            var user = await GetCurrentUser();
            if (user.Code != model.code)
                return ResponseHelper.Failure<UserInfoDTO>(HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeNotCorrect.ToNumber(), model.lang));

            user.Email = model.email;

            await db.SaveChangesAsync();

            var token = _helper.GenerateToken(user);
            var userInfo = await _userService.GetUserInfo(user.Id, model.lang, token);
            return ResponseHelper.Success<UserInfoDTO>(userInfo.Data, HelperMsg.MsgValidation(EnumValidMsg.Auth.UpdateSuccessfully.ToNumber(), model.lang));
        }
        public async Task<BaseResponseDto<object>> ChangeLanguage(string lang)
        {
            var userId = _currentUserService.UserId;
            var Users = await db.Users.FindAsync(userId);
            if (Users != null)
            {
                Users.Lang = lang;
                await db.SaveChangesAsync();
                return ResponseHelper.Success<object>(true, HelperMsg.MsgValidation(EnumValidMsg.Auth.UpdateSuccessfully.ToNumber(), lang));
            }
            else
            {
                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), lang));
            }
        }
        public async Task<BaseResponseDto<object>> Logout(LogoutDto userModel)
        {
            userModel.lang = userModel.lang ?? "ar";
            var info = await db.DeviceIds.Where(st => st.UserId == _currentUserService.UserId).ToListAsync();
            if (info.Count > 0)
            {
                foreach (var item in info)
                {
                    db.DeviceIds.Remove(item);
                    await db.SaveChangesAsync();
                }
                return ResponseHelper.Success<object>(true, HelperMsg.MsgValidation(EnumValidMsg.Auth.LogOutSuccessfully.ToNumber(), userModel.lang));
            }
            else
            {
                return ResponseHelper.Success<object>(true, HelperMsg.MsgValidation(EnumValidMsg.Auth.LogOutSuccessfully.ToNumber(), userModel.lang));
            }

        }
        #endregion
        #region Provider
        public async Task<BaseResponseDto<ProviderInfoDto>> AddProvider(AddProviderViewModel model)
        {
            //var provider = model.Adapt<ApplicationDbUser>();
            var provider = MapsterMapping.MapAddProviderViewModelToApplicationDbUser(model);
            provider.ShowPassword = model.Password;
            string imageProfile = "";
            if (model.ImageProfile is not null)
                //imageProfile = _helper.Upload(model.ImageProfile, (int)FileName.Users);
                imageProfile = await _helper.UploadFileUsingApi(new UploadImageDto { image = model.ImageProfile, fileName = (int)FileName.Users });
            else
                imageProfile = "images/Users/default.jpg";

            provider.ImgProfile = imageProfile;

            #region validation
            bool userWithSamePhoneNumber = await _userService.CheckValidatePhone(model.PhoneNumber, model.PhoneCode);
            if (userWithSamePhoneNumber)
                return ResponseHelper.Failure<ProviderInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.PhoneExisting.ToNumber(), model.Lang));
            #endregion
            int code = await GenerateCode(1234);
            provider.Code = code;
            var result = await _userManager.CreateAsync(provider, provider.ShowPassword);
            if (result.Succeeded)
            {

                var userInfo = await _userService.GetProviderInfo(provider.Id, model.Lang, "");

                
                _ = await SendSms(code, provider.PhoneNumber);
                return ResponseHelper.Success(userInfo, HelperMsg.MsgValidation(EnumValidMsg.Auth.RegisterSuccessfully.ToNumber(), model.Lang));
            }
            return ResponseHelper.Failure<ProviderInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), model.Lang), 400, "failed", result.Errors.Select(c => new ValidationError
            {
                ErrorMessage = c.Description,
                PropertyName = c.Code
            }).ToList());


        }
        public async Task<BaseResponseDto<ChangePhoneResponse>> ChangeProviderPhoneNumber(ChangePhoneNumberViewModel model)
        {
            var currentUser = await db.Users.FirstOrDefaultAsync(c => c.Id == model.UserId);
            var passwordCheck = await CheckPassword(currentUser, model.CurrentPasword, model.lang);

            if (!passwordCheck.IsSuccess)
                return ResponseHelper.Failure<ChangePhoneResponse>(passwordCheck.Message);

            var validatePhoneNumber = await _userService.CheckValidatePhone(model.PhoneNumber, model.PhoneCode);

            if (validatePhoneNumber)
                return ResponseHelper.Failure<ChangePhoneResponse>(HelperMsg.MsgValidation(EnumValidMsg.Auth.PhoneExisting.ToNumber(), model.lang));

            int code = await GenerateCode(1234);
            currentUser.Code = code;
            await db.SaveChangesAsync();
            _ = await SendSms(code, model.PhoneNumber);
            return ResponseHelper.Success(new ChangePhoneResponse { Code = code, PhoneCode = model.PhoneCode, PhoneNumber = model.PhoneNumber },
                HelperMsg.MsgValidation(EnumValidMsg.Auth.SendSuccessfully.ToNumber(), model.lang));
        }
        public async Task<BaseResponseDto<object>> CheckChangePhoneCode(CheckChangePhoneCodeViewModel model)
        {
            var currentUser = await db.Users.FirstOrDefaultAsync(c => c.Id == model.UserId);

            if (model.Code != currentUser.Code)
                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeNotCorrect.ToNumber(), model.lang));

            currentUser.PhoneCode = model.PhoneCode;
            currentUser.PhoneNumber = model.PhoneNumber;
            await db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.MsgValidation(EnumValidMsg.Auth.UpdateSuccessfully.ToNumber(), model.lang));

        }
        public async Task<BaseResponseDto<object>> AddCapatin(AddCapationViewModel model)
        {
            var addCaptain = MapsterMapping.MapToNewCaptain(model);
            #region validation

            bool userWithSamePhoneNumber = await _userService.CheckValidatePhone(model.Phone, model.PhoneCode);
            if (userWithSamePhoneNumber)
                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.PhoneExisting.ToNumber(), model.lang));

            bool userWithSameEmail = await _userService.CheckValidateEmail(model.Email);
            if (userWithSameEmail)
                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.EmailExisting.ToNumber(), model.lang));
            #endregion

            //addCaptain.ImgProfile = _helper.Upload(model.ImageProfile, (int)FileName.Users);
            addCaptain.ImgProfile = await _helper.UploadFileUsingApi(new UploadImageDto { image = model.ImageProfile, fileName = (int)FileName.Users });
            addCaptain.IdentityNumberImage = await _helper.UploadFileUsingApi(new UploadImageDto { image = model.IdentityNumberImage, fileName = (int)FileName.Users });
            addCaptain.DeliveryLicenseImage = await _helper.UploadFileUsingApi(new UploadImageDto { image = model.DeliveryLicenseImage, fileName = (int)FileName.Users });
            var result = await _userManager.CreateAsync(addCaptain, addCaptain.ShowPassword);
            if (result.Succeeded)
            {
                return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تمت إضافة الكابتن بنجاح", "Captain added successfully"));
            }
            return ResponseHelper.Failure<object>(HelperMsg.creatMessage(model.lang, "حدث خطا ما", "Something went wrong"));
        }
        public async Task<BaseResponseDto<List<GetCampanyCaptainsViewModel>>> GetDeliveryCompanyCaptains(string deliverCompanyId, string lang)
        {
            var cultureInfo = Helper.GetCulture(lang);
            var captains = await db.Users
                 .Include(c => c.Nationality)
                 .Where(c => c.DeliverCompanyId == deliverCompanyId)
                 .Select(c => new GetCampanyCaptainsViewModel
                 {
                     Id = c.Id,
                     ImageProfile = DashBordUrl.baseUrlHost + c.ImgProfile,
                     Phone = c.PhoneNumber + " " + c.PhoneCode,
                     UserName = c.user_Name,
                     CreationDate = c.PublishDate.ToString("dd/MM/yyyy", cultureInfo)
                 }).ToListAsync();
            return ResponseHelper.Success(captains);
        }
        public async Task<BaseResponseDto<CaptionDetailsViewModel>> GetCaptainDetails(string id, string lang)
        {
            var captainDetails = await db.Users
                 .Include(c => c.city)
                 .Include(c => c.Nationality)
                 .Where(c => c.Id == id)
                 .Select(c => new CaptionDetailsViewModel
                 {
                     Id = c.Id,
                     UserName = c.user_Name,
                     Email = c.Email,
                     PhoneNumber = c.PhoneNumber + "" + c.PhoneCode,
                     // Age = c.Age ?? c.Age.Value,
                     BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                     City = c.city.ChangeLangName(lang),
                     DeliverLicense = DashBordUrl.baseUrlHost + c.DeliveryLicenseImage,
                     IdentityNumberImage = DashBordUrl.baseUrlHost + c.IdentityNumberImage,
                     ImageProfile = DashBordUrl.baseUrlHost + c.ImgProfile,
                     Gender = Helper.GetGenderTypeText(c.GenderType.Value, lang),
                     Nationality = c.Nationality.ChangeLangName(lang),
                     IdentityNumber = c.IdentityNumber,
                     Password = c.ShowPassword
                 }).FirstOrDefaultAsync();
            return ResponseHelper.Success(captainDetails);
        }
        public async Task<BaseResponseDto<EditCaptainViewModel>> GetCaptionToUpdate(string id, string lang)
        {
            var captainDetails = await db.Users
                .Include(c => c.city)
                .Include(c => c.Nationality)
                .Where(c => c.Id == id)
                .Select(c => new EditCaptainViewModel
                {
                    Id = c.Id,
                    UserName = c.user_Name,
                    Email = c.Email,
                    //PhoneNumber = c.PhoneNumber,
                    //PhoneCode = c.PhoneCode,
                    //Age = c.Age ?? c.Age.Value,
                    BirthDate = c.BirthDate,
                    CityId = c.CityId,
                    DeliveryImage = DashBordUrl.baseUrlHost + c.DeliveryLicenseImage,
                    IdentityImage = DashBordUrl.baseUrlHost + c.IdentityNumberImage,
                    ImageProfile = DashBordUrl.baseUrlHost + c.ImgProfile,
                    GenderType = c.GenderType.Value,
                    Nationality = c.NantionalityId.Value,
                    IdentityNumber = c.IdentityNumber,
                }).FirstOrDefaultAsync();
            return ResponseHelper.Success(captainDetails);
        }
        public async Task<BaseResponseDto<object>> UpdateCaption(EditCaptainViewModel model)
        {
            var caption = await db.Users.FirstOrDefaultAsync(c => c.Id == model.Id);
            caption.user_Name = model.UserName;
            caption.Email = model.Email;
            caption.CityId = model.CityId;
            caption.NantionalityId = model.Nationality;
            caption.GenderType = model.GenderType;
            //caption.Age = model.Age;
            caption.BirthDate = model.BirthDate;
            caption.IdentityNumber = model.IdentityNumber;
            caption.ImgProfile = model.NewImageProfile != null ? await _helper.UploadFileUsingApi(new UploadImageDto { image = model.NewImageProfile, fileName = (int)FileName.Users }) : caption.ImgProfile;
            caption.IdentityNumberImage = model.NewIdentityImage != null ? await _helper.UploadFileUsingApi(new UploadImageDto { image = model.NewIdentityImage, fileName = (int)FileName.Users }) : caption.ImgProfile;
            caption.DeliveryLicenseImage = model.NewDliveryLisence != null ? await _helper.UploadFileUsingApi(new UploadImageDto { image = model.NewDliveryLisence, fileName = (int)FileName.Users }) : caption.ImgProfile;
            db.Users.Update(caption);
            try
            {
                await db.SaveChangesAsync();
                return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تم تعديل بيانات الكابتن بنجاح", "The Captain data has been successfully modified"));
            }
            catch (Exception)
            {

                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), model.lang));
            }

        }
        public async Task<BaseResponseDto<object>> DeleteCapation(string captaionId, string lang)
        {
            var deletedCaptain = await db.Users.FirstOrDefaultAsync(c => c.Id == captaionId);
            if (deletedCaptain is not null)
            {
                var checkHasOrder = await db.Orders.AnyAsync(c => c.UserId == captaionId
                && (c.OrderStatus == (int)OrderStutes.NewOrder || c.OrderStatus == (int)OrderStutes.Current));
                if (checkHasOrder)
                {
                    return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "لا يمكنك حذف كابتن لديه طلبات", "You cannot delete a captain who has requests"));
                }
                deletedCaptain.PhoneNumber = deletedCaptain.PhoneNumber + Guid.NewGuid().ToString();
                deletedCaptain.Email = deletedCaptain.Email + Guid.NewGuid().ToString();
                deletedCaptain.NormalizedEmail = deletedCaptain.NormalizedEmail + Guid.NewGuid().ToString();
                deletedCaptain.UserName = deletedCaptain.UserName + Guid.NewGuid().ToString();
                deletedCaptain.IsDeleted = true;
                await db.SaveChangesAsync();
                return ResponseHelper.Success<object>(true, HelperMsg.MsgValidation(EnumValidMsg.Auth.DeleteSuccessfully.ToNumber(), lang));
            }

            return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.UserNotFound.ToNumber(), lang));
        }
        public async Task<BaseResponseDto<ForgetPasswordDto>> ForgetProviderPassword(ForgetPasswordViewModel model)
        {
            model.lang = model.lang ?? "ar";
            var user = await db.Users
                .FirstOrDefaultAsync(c => c.PhoneNumber == model.phone && c.PhoneCode == model.phoneCode);
            if (user is null)
                return ResponseHelper.Failure<ForgetPasswordDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.PhoneNotFound.ToNumber(), model.lang));

            if (!(await CheckAccountBlocked(user)))
                return ResponseHelper.Failure<ForgetPasswordDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.Accountblocked.ToNumber(), model.lang));

            int code = await GenerateCode(1234);
            user.Code = code;
            await db.SaveChangesAsync();
            _ = await SendSms(code, user.PhoneNumber);
            var data = new ForgetPasswordDto { code = code, userId = user.Id };
            return ResponseHelper.Success(data);
        }

        #endregion

        #region Shared
        public async Task<BaseResponseDto<BaseInfoDto>> ConfirmCodeRegister(ConfirmCodeAddDto confirmCodeAddDto)
        {

            var foundedUser = await db.Users.FirstOrDefaultAsync(c => c.Id == confirmCodeAddDto.userId);
            if (foundedUser != null)
            {
                if (foundedUser.Code == confirmCodeAddDto.code)
                {
                    foundedUser.ActiveCode = true;
                    await db.SaveChangesAsync();
                    var token = _helper.GenerateToken(foundedUser);
                    if (foundedUser.TypeUser == (int)UserType.Client)
                    {
                        var userInfo = await _userService.GetUserInfo(foundedUser.Id, confirmCodeAddDto.lang, token);
                        return ResponseHelper.Success<BaseInfoDto>(userInfo.Data);
                    }
                    else
                    {
                        var providerInfo = await _userService.GetProviderInfo(foundedUser.Id, confirmCodeAddDto.lang, token);
                        return ResponseHelper.Success<BaseInfoDto>(providerInfo);
                    }


                }
                else
                {
                    return ResponseHelper.Failure<BaseInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeNotCorrect.ToNumber(), confirmCodeAddDto.lang));
                }

            }
            else
            {
                return ResponseHelper.Failure<BaseInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.UserNotFound.ToNumber(), confirmCodeAddDto.lang));
            }
        }
        public async Task<BaseResponseDto<ResendCodeDto>> ResendCode(ResendCodeAddDto resendCodeAddDto)
        {
            var foundedUser = await db.Users.FindAsync(resendCodeAddDto.userId);
            if (foundedUser != null)
            {
                int code = await GenerateCode(1234);
                _ = await SendSms(code, foundedUser.PhoneNumber);
                foundedUser.Code = code;
                await db.SaveChangesAsync();
                var model = new ResendCodeDto { code = code, phone = foundedUser.PhoneNumber, userId = foundedUser.Id };

                return ResponseHelper.Success(model, HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeSent.ToNumber(), resendCodeAddDto.lang));

            }
            else
            {

                // return new ResendCodeDto { userId = "", phone = "", code = 0 };
                return ResponseHelper.Failure<ResendCodeDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), resendCodeAddDto.lang));
            }
        }

        public async Task<BaseResponseDto<BaseInfoDto>> Login(LoginDto loginDto)
        {
            loginDto.lang = loginDto.lang ?? "ar";
            #region Validation
            //var user = await GetUserUsingPhone(loginDto.phone, loginDto.phoneCode);
            var user = await db.Users
                .SingleOrDefaultAsync(c => c.PhoneNumber == loginDto.phone && c.PhoneCode == loginDto.phoneCode && c.TypeUser == loginDto.type);
            if (user == null)
                return ResponseHelper.Failure<BaseInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.UserNotFound.ToNumber(), loginDto.lang));

            if (await ValidatePassword(loginDto.password, user.ShowPassword))
                return ResponseHelper.Failure<BaseInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.PasswordNotFound.ToNumber(), loginDto.lang));

            if (!(await CheckIsActiveOrNo(user)))
            {
                _ = await SendSms(user.Code, user.PhoneNumber);
                //return ResponseHelper.Failure<BaseInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.NotActive.ToNumber(), loginDto.lang));
                if (user.TypeUser == UserType.Client.ToNumber())
                {
                    var data = await _userService.GetUserInfo(user.Id, loginDto.lang, "");
                    return ResponseHelper.Success<BaseInfoDto>(data.Data, HelperMsg.MsgValidation(EnumValidMsg.Auth.NotActive.ToNumber(), loginDto.lang));
                }
                else
                {
                    var data = await _userService.GetProviderInfo(user.Id, loginDto.lang, "");
                    return ResponseHelper.Success<BaseInfoDto>(data, HelperMsg.MsgValidation(EnumValidMsg.Auth.NotActive.ToNumber(), loginDto.lang));
                }
            }


            if (!(await CheckAccountBlocked(user)))
                return ResponseHelper.Failure<BaseInfoDto>(HelperMsg.creatMessage(loginDto.lang,"تم حظر حسابك لاعادة تفعيله يرجي التواصل مع الدعم الفني", "Your account has been blocked. To reactivate it, please contact technical support"));
           
            var checkPasswordResult = await CheckPassword(user, loginDto.password, loginDto.lang);
            if (checkPasswordResult.IsSuccess == true)
            {
                var token = _helper.GenerateToken(user);

                var deviceModel = loginDto.Adapt<DeviceIdAddDto>();
                deviceModel.userId = user.Id;
                await AddDeviceId(deviceModel);
                if (user.TypeUser == UserType.Client.ToNumber())
                {
                    var data = await _userService.GetUserInfo(user.Id, loginDto.lang, token);
                    return ResponseHelper.Success<BaseInfoDto>(data.Data);
                }
                else
                {
                    var data = await _userService.GetProviderInfo(user.Id, loginDto.lang, token);
                    return ResponseHelper.Success<BaseInfoDto>(data);
                }
            }
            else
                return ResponseHelper.Failure<BaseInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.PasswordNotFound.ToNumber(), loginDto.lang));
            #endregion
        }
        public async Task<BaseResponseDto<CheckCodeDto>> CheckCode(string userId, int code, string lang)
        {
            var user = await GetUserUsingId(userId);

            if (user.Code != code)
                return ResponseHelper.Failure<CheckCodeDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeNotCorrect.ToNumber(), lang));

            return ResponseHelper.Success(new CheckCodeDto { userId = user.Id }, HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeActivatedSuccessfully.ToNumber(), lang));
        }
        public async Task<BaseResponseDto<object>> ChangePasswordByCode(ChangePasswordByCodeDto changePasswordByCodeDto)
        {
            changePasswordByCodeDto.lang = changePasswordByCodeDto.lang ?? "ar";

            var user = await GetUserUsingId(changePasswordByCodeDto.userId);

            var checkPasssword = await ValidatePassword(changePasswordByCodeDto.newPassword, user.ShowPassword);
            if (!checkPasssword)
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(changePasswordByCodeDto.lang, "كلمه المرور الجديده لا يجب ان تساوي كلمة المرور القديمة", "The new password does not have to be equal to the old password"));


            var changePasswordResult = await _userManager.ChangePasswordAsync(user, user.ShowPassword, changePasswordByCodeDto.newPassword);
            if (!changePasswordResult.Succeeded)

                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.NewPasswordNotCorrect.ToNumber(), changePasswordByCodeDto.lang));

            user.ShowPassword = changePasswordByCodeDto.newPassword;
            await db.SaveChangesAsync();

            return ResponseHelper.Success<object>(true, HelperMsg.MsgValidation(EnumValidMsg.Auth.Passwordchanged.ToNumber(), changePasswordByCodeDto.lang));
        }
        public async Task<BaseResponseDto<object>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            changePasswordDto.lang = changePasswordDto.lang ?? "ar";
            //var user = (await _userManager.FindByIdAsync(_currentUserService.UserId));
            var user = await db.Users.FirstOrDefaultAsync(c => c.Id == changePasswordDto.userId);

            if (await ValidatePassword(user.ShowPassword, changePasswordDto.oldPassword))
                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.OldPasswordNotCorrect.ToNumber(), changePasswordDto.lang));

            if (!await ValidatePassword(user.ShowPassword, changePasswordDto.newPassword))
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(changePasswordDto.lang, "كلمه المرور الجديده لا يجب ان تساوي كلمة المرور القديمة", "The new password does not have to be equal to the old password"));

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordDto.oldPassword, changePasswordDto.newPassword);
            if (!changePasswordResult.Succeeded)
                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.NewPasswordNotCorrect.ToNumber(), changePasswordDto.lang));

            user.ShowPassword = changePasswordDto.newPassword;
            await db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.MsgValidation(EnumValidMsg.Auth.Passwordchanged.ToNumber(), changePasswordDto.lang));
        }
        public async Task<BaseResponseDto<ChangePhoneResponseDto>> ChangePhoneNumber(ChangePhoneDto model)
        {
            var user = await GetUserUsingId(model.userId);
            var passwordCheck = await CheckPassword(user, model.password, model.lang);

            if (!passwordCheck.IsSuccess)
                return ResponseHelper.Failure<ChangePhoneResponseDto>(passwordCheck.Message);

            bool validateNewPhoneNumber = await _userService.CheckValidatePhone(model.phone, model.phoneCode);

            if (validateNewPhoneNumber)
                return ResponseHelper.Failure<ChangePhoneResponseDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.PhoneExisting.ToNumber(), model.lang));

            int code = await GenerateCode(1234);
            user.Code = code;
            await db.SaveChangesAsync();

            _ = SendSms(code, user.PhoneNumber);

            var data = new ChangePhoneResponseDto { code = user.Code, newPhoneNumber = model.phone, phoneCode = model.phoneCode, userId = user.Id };

            return ResponseHelper.Success(data, HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeSent.ToNumber(), model.lang));
        }
        public async Task<BaseResponseDto<BaseInfoDto>> ConfirmChangePhoneNumber(ConfirmChangePhoneDto model)
        {
            var user = await GetUserUsingId(model.userId);
            if (user.Code != model.code)
                return ResponseHelper.Failure<BaseInfoDto>(HelperMsg.MsgValidation(EnumValidMsg.Auth.CodeNotCorrect.ToNumber(), model.lang));

            user.PhoneNumber = model.phoneNumber;
            user.PhoneCode = model.phoneCode;
            await db.SaveChangesAsync();

            var token = _helper.GenerateToken(user);

            if (user.TypeUser == UserType.Client.ToNumber())
            {
                var userInfo = await _userService.GetUserInfo(user.Id, model.lang, token);
                return ResponseHelper.Success<BaseInfoDto>(userInfo.Data, HelperMsg.MsgValidation(EnumValidMsg.Auth.UpdateSuccessfully.ToNumber(), model.lang));
            }
            else
            {
                var data = await _userService.GetProviderInfo(user.Id, model.lang, token);
                return ResponseHelper.Success<BaseInfoDto>(data, HelperMsg.MsgValidation(EnumValidMsg.Auth.UpdateSuccessfully.ToNumber(), model.lang));
            }
        }
        #endregion

        public async Task<bool> AddUserToRole(ApplicationDbUser userInfoAddDTO, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(userInfoAddDTO, roleName)).Succeeded;
            return result;
        }



        public async Task<bool> CheckAccountBlocked(ApplicationDbUser user)
        {

            return user.IsActive;
        }

        public async Task<bool> CheckIsActiveOrNo(ApplicationDbUser user)
        {
            return user.ActiveCode;
        }

        public async Task<BaseResponseDto<object>> CheckPassword(ApplicationDbUser user, string password, string lang)
        {
            bool isCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (isCorrect)
                return ResponseHelper.Success<object>(isCorrect, HelperMsg.creatMessage(lang, "تم التاكد من كلمه المرور", "Password Is Correct"));

            return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.PasswordNotFound.ToNumber(), lang));
        }

        public async Task<bool> CheckValidateEmail(string email)
        {

            ApplicationDbUser foundedEmail = await _userManager.FindByEmailAsync(email);
            return foundedEmail != null;
        }

        public async Task<bool> CheckValidatePhone(string phone)
        {

            ApplicationDbUser foundedPhone = await db.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phone);
            return foundedPhone != null;
        }





        public async Task<bool> ValidatePassword(string password, string showPassword)
        {
            return password != showPassword;
        }
        public async Task<BaseResponseDto<object>> RemoveAccount(string currentUserId, string lang = "ar")
        {
         

            var foundedUser = await db.Users.FindAsync(currentUserId);
            if (foundedUser == null)
            {
                return ResponseHelper.Failure<object>(HelperMsg.MsgValidation(EnumValidMsg.Auth.UserNotFound.ToNumber(), lang));
            }
            foundedUser.user_Name = foundedUser.user_Name + Guid.NewGuid().ToString();
            foundedUser.PhoneNumber = foundedUser.PhoneNumber + Guid.NewGuid().ToString();
            foundedUser.Email = foundedUser.Email + Guid.NewGuid().ToString();
            foundedUser.NormalizedEmail = foundedUser.NormalizedEmail + Guid.NewGuid().ToString();
            foundedUser.UserName = foundedUser.UserName + Guid.NewGuid().ToString();
            foundedUser.NormalizedUserName = foundedUser.NormalizedUserName + Guid.NewGuid().ToString();
            foundedUser.IsDeleted = true;

            await db.SaveChangesAsync();

            return ResponseHelper.Success<object>(true, HelperMsg.MsgValidation(EnumValidMsg.Auth.DeleteSuccessfully.ToNumber(), lang));
        }

        public async Task<ApplicationDbUser> GetUserUsingPhone(string PhoneNumber, string phoneCode)
        {
            string englishPhoneNumber = HelperNumber.ConvertArabicNumberToEnglish(PhoneNumber);
            return await db.Users.Where(x => x.PhoneNumber == englishPhoneNumber && x.PhoneCode == phoneCode).SingleOrDefaultAsync();
        }
        public async Task<ApplicationDbUser> GetUserByEmail(string email)
        {
            var user = await db.Users.SingleOrDefaultAsync(c => c.Email == email);
            return user;
        }
        public async Task<ApplicationDbUser> GetUserUsingId(string UserId)
        {
            return await db.Users.SingleOrDefaultAsync(x => x.Id == UserId);
        }



        public async Task<bool> UpdateCode(string UserPhone, string phoneCode, int Code)
        {
            var user = await GetUserUsingPhone(UserPhone, phoneCode);
            user.Code = Code;
            return await db.SaveChangesAsync() > 0;
        }






        #region Helper Method
        public async Task<int> GenerateCode(int currentCode)
        {
            try
            {
                int code = HelperNumber.GetRandomNumber(currentCode);
                Setting GetInfoSms = await db.Settings.FirstOrDefaultAsync();
                if (GetInfoSms != null)
                {
                    if (GetInfoSms.SenderName != "test")
                    {
                        code = HelperNumber.GetRandomNumber();
                    }
                }
                return code;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public async Task<string> SendSms(int code, string phone)
        {
            Setting GetInfoSms = await db.Settings.FirstOrDefaultAsync();
            if (GetInfoSms != null)
            {
                if (GetInfoSms.SenderName != "test")
                {
                    //string resultSms = await HelperSms.SendMessageBy4jawaly(code.ToString(), phone, GetInfoSms.SenderName, GetInfoSms.UserNameSms, GetInfoSms.PasswordSms);
                    //return resultSms;
                    var model = new Root()
                    {
                        numbers = "0"+phone,
                        text = code.ToString(),
                        app_key = "9PPcBdf2nRwU2ISD7sAsJYyNag1KzNOkYKYOh32J",
                        app_secret = "LiraqSq49qBdVu2q0I4O7j3yFFE7AuA0JfNQ1C91IcvxE50UU3BtyCyMPTDM8zNTQdEjdW0DgpC6nSprkXOdAh8B7h96lXJzRerD",
                        sender = "Mumaken"
                    };
                    var result = await _4jawalySmsHelper.SendSms(model);
                    return result.message;
                }
            }
            return "";
        }
        public async Task<ApplicationDbUser> GetCurrentUser()
        {
            var userId = _currentUserService.UserId;
            var currentUser = await db.Users.FirstOrDefaultAsync(c => c.Id == userId);
            return currentUser;
        }
        #endregion
    }
}
