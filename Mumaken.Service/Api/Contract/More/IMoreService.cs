using Microsoft.AspNetCore.Mvc.Rendering;
using Mumaken.Domain.DTO;
using Mumaken.Domain.DTO.MoreDto;
using Mumaken.Domain.ViewModel.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Api.Contract.More
{
    public interface IMoreService
    {
        Task<BaseResponseDto<List<IntroSplashScreenDto>>> GetIntroScreen(string lang);
        Task<BaseResponseDto<string>> GetAboutApp(string lang);
        Task<BaseResponseDto<string>>GetClientAboutUs(string lang);
        Task<BaseResponseDto<string>> GetClientPrivacyPolicy(string lang);
        Task<BaseResponseDto<string>> GetClientTermsAndCondition(string lang);
        Task<BaseResponseDto<string>> GetProviderTermsAndCondition(string lang);
        Task<BaseResponseDto<List<CommonQuestionDto>>> GetCommonQuestion(string lang);
        Task<BaseResponseDto<GetContactInfoDto>> GetContactInfo(string lang);
        Task<BaseResponseDto<object>> AddContactUs(ContactUsDto model);
        Task<BaseResponseDto<List<GetAllCityDto>>> GetAllCity(string lang);
        Task<List<SelectListItem>> GetCitiesSelectList(int? cityId, string lang = "ar");
        Task<List<RegoinsIndexViewModel>> GetAllRegionIncity(int cityId, string lang = "ar");
        Task<List<SelectListItem>> GetAllRegionIncitySelectedList(int cityId, int? regionId, string lang = "ar");
        Task<List<SelectListItem>> GetGenderType(int? selectedGender, string lang = "ar");
        Task<List<SelectListItem>> GetNationality(int? nationalitySelected, string lang = "ar");
        Task<BaseResponseDto<string>> GetAboutAppProvider(string lang);
    }
}
