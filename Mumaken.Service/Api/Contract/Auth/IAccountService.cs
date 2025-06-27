using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.Entities.UserTables;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mumaken.Domain.DTO;
using Mumaken.Domain.ViewModel.Provider;
using Mumaken.Domain.ViewModel.Auth;

namespace Mumaken.Service.Api.Contract.Auth
{
    public interface IAccountService
    {


        Task<BaseResponseDto<UserInfoDTO>> AddUser(UserAddDTO model);

        Task<BaseResponseDto<BaseInfoDto>> ConfirmCodeRegister(ConfirmCodeAddDto confirmCodeAddDto);
        Task<BaseResponseDto<ResendCodeDto>> ResendCode(ResendCodeAddDto resendCodeAddDto);
        Task<BaseResponseDto<BaseInfoDto>> Login(LoginDto loginDto);

        Task<BaseResponseDto<ForgetPasswordDto>> ForgetUserPassword(ForgetPasswordAddDto model);
        Task<BaseResponseDto<CheckCodeDto>> CheckCode(string userId, int code, string lang);
        Task<BaseResponseDto<ChangePhoneResponseDto>> ChangePhoneNumber(ChangePhoneDto model);

        Task<BaseResponseDto<BaseInfoDto>> ConfirmChangePhoneNumber(ConfirmChangePhoneDto model);

        Task<BaseResponseDto<ChangeEmailResponseDto>> ChangeEmail(ChangeEmailDto model);
        Task<BaseResponseDto<UserInfoDTO>> ConfirmChangeEmail(ConfirmChangeEmailDto model);
        Task<int> GenerateCode(int currentCode);
        Task<bool> AddDeviceId(DeviceIdAddDto deviceIdAddDto);
        Task<string> SendSms(int code, string phone);
        
        Task<bool> AddUserToRole(ApplicationDbUser userInfoAddDTO, string roleName);


        Task<BaseResponseDto<object>> CheckPassword(ApplicationDbUser user, string password,string lang);
        Task<bool> ValidatePassword(string password, string showPassword);
        Task<bool> CheckAccountBlocked(ApplicationDbUser user);
        Task<bool> CheckIsActiveOrNo(ApplicationDbUser user);
        Task<BaseResponseDto<object>> ChangeLanguage(string lang);
        Task<BaseResponseDto<object>> RemoveAccount(string currentUserId, string lang);
        Task<ApplicationDbUser> GetUserUsingPhone(string PhoneNumber,string phoneCode);
        Task<ApplicationDbUser> GetUserByEmail(string email);
        Task<ApplicationDbUser> GetUserUsingId(string UserId);
        Task<BaseResponseDto<object>> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<bool> UpdateCode(string UserPhone,string phoneCode, int Code);
        Task<BaseResponseDto<object>> ChangePasswordByCode(ChangePasswordByCodeDto changePasswordByCodeDto);
        Task<BaseResponseDto<object>> Logout(LogoutDto userModel);
        Task<ApplicationDbUser> GetCurrentUser();
        Task<BaseResponseDto<ProviderInfoDto>> AddProvider(AddProviderViewModel model);
        Task<BaseResponseDto<ChangePhoneResponse>> ChangeProviderPhoneNumber(ChangePhoneNumberViewModel model);
        Task<BaseResponseDto<object>> CheckChangePhoneCode(CheckChangePhoneCodeViewModel model);
        Task<BaseResponseDto<object>> AddCapatin(AddCapationViewModel model);
        Task<BaseResponseDto<List<GetCampanyCaptainsViewModel>>> GetDeliveryCompanyCaptains(string deliverCompanyId, string lang);
        Task<BaseResponseDto<object>> DeleteCapation(string captaionId, string lang);
        Task<BaseResponseDto<CaptionDetailsViewModel>> GetCaptainDetails(string id, string lang);
        Task<BaseResponseDto<EditCaptainViewModel>> GetCaptionToUpdate(string id, string lang);
        Task<BaseResponseDto<object>> UpdateCaption(EditCaptainViewModel model);
        Task<BaseResponseDto<ForgetPasswordDto>> ForgetProviderPassword(ForgetPasswordViewModel model);
    }
}
