using Mumaken.Domain.DTO;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.ViewModel.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Api.Contract.Auth
{
    public interface IUserServices
    {
        Task<BaseResponseDto<UserInfoDTO>> GetUserInfo(string userId, string lang, string token);
        Task<BaseResponseDto<UserInfoDTO>> UpdateDataUser(UpdateDataUserDto updateDataUserDto);
        Task<BaseResponseDto<object>> GetUserWalet();

        Task<BaseResponseDto<object>> DeleteAllNotify(string lang = "ar");
        Task<BaseResponseDto<object>> DeleteNotifyById(int notifyId, string lang = "ar");
        Task<bool> ChangeNotify(ChangeNotifyEditDto changeNotifyEditDto);
        Task<bool> AddUserNotify(string textAr, string textEn, string userId, int stutes, int orderId = 0);
        Task<bool> SendNotify(string textAr, string textEn, string fkProvider, int stutes, int orderId = 0);
        Task<bool> SendNotifyAsync(string textAr, string textEn, string userId, int stutes);
        Task<BaseResponseDto<List<NotifyListDto>>> ListOfNotifyUser(string userId, string lang = "ar");
        Task<bool> CheckValidatePhone(string phone,string phoneCode);
        Task<bool> CheckValidateEmail(string email);
  
        Task<ProviderInfoDto> GetProviderInfo(string userId, string lang, string token);
        Task<BaseResponseDto<ProviderInfoDto>> UpdateProvider(UpadateProviderViewModel model);
        Task<BaseResponseDto<object>> DeleteAllDelgateNotify(string providerId, string lang = "ar");
        Task<BaseResponseDto<object>> DeleteDelgateNotifyById(int notifyId, string lang = "ar");
        Task<BaseResponseDto<List<NotifyListDto>>> ListOfNotifyProvider(string userId, string lang = "ar");

        Task<BaseResponseDto<object>> ChangeState(string id);
        Task<BaseResponseDto<object>> DeleteAccount(string id, string lang = "ar");
        Task<BaseResponseDto<object>> AcceptRequestJoin(string id, string lang = "ar");

    }
}
