using AAITHelper;
using AAITHelper.Enums;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.Helpers.ResponseHelper;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO;
using Mumaken.Domain.DTO.MoreDto;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Region;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.More;
using Mumaken.Service.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Api.Implementation.More
{
    public class MoreService : IMoreService
    {
        private readonly ApplicationDbContext _db;

        public MoreService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<BaseResponseDto<string>> GetAboutApp(string lang)
        {
            var aboutApp = await _db.Settings
                .Select(c => c.ChangeLangAboutApp(lang))
                .FirstOrDefaultAsync();
            return ResponseHelper.Success(aboutApp!);
        }
        public async Task<BaseResponseDto<string>> GetAboutAppProvider(string lang)
        {
            var aboutApp = await _db.Settings
                .Select(c => c.ChangeLangAboutAppProvider(lang))
                .FirstOrDefaultAsync();
            return ResponseHelper.Success(aboutApp!);
        }

        public async Task<BaseResponseDto<string>> GetClientAboutUs(string lang)
        {
            var aboutUsClient = await _db.Settings
               .Select(c => c.ChangeLangAboutUsClient(lang))
               .FirstOrDefaultAsync();
            return ResponseHelper.Success(aboutUsClient!);
        }

        public async Task<BaseResponseDto<string>> GetClientPrivacyPolicy(string lang)
        {
            var privacyPolicyClient = await _db.Settings
               .Select(c => c.ChangeLangPrivacyPolicy(lang))
               .FirstOrDefaultAsync();
            return ResponseHelper.Success(privacyPolicyClient!);
        }

        public async Task<BaseResponseDto<string>> GetClientTermsAndCondition(string lang)
        {
            var conditionAndTermClient = await _db.Settings
               .Select(c => c.ChangeLangCondtionClient(lang))
               .FirstOrDefaultAsync();
            return ResponseHelper.Success(conditionAndTermClient!);
        }
     

        public async Task<BaseResponseDto<List<CommonQuestionDto>>> GetCommonQuestion(string lang)
        {
            var commonQuestion = await _db.CommonQuestions
                  .Where(c => c.IsActive)
                  .Select(c => new CommonQuestionDto
                  {
                      id = c.Id,
                      answer = c.ChangeLangAnswer(lang),
                      question = c.ChangeLangQuestion(lang)
                  }).ToListAsync();
            return ResponseHelper.Success(commonQuestion);
        }
        public async Task<BaseResponseDto<List<IntroSplashScreenDto>>> GetIntroScreen(string lang)
        {

            var introScreens = await _db.SplashScreens
                .Where(c => c.IsActive)
               .Select(c => new IntroSplashScreenDto
               {
                   Id = c.Id,
                   Img = DashBordUrl.baseUrlHost + lang=="ar"? c.ImgAr:c.ImgEn,
                   Description = c.ChangeLangDescription(lang),
                   Title = c.ChangeLangTitle(lang),
               }).ToListAsync();

            return ResponseHelper.Success(introScreens);
        }
        public async Task<BaseResponseDto<GetContactInfoDto>> GetContactInfo(string lang)
        {
            var contactInfo = await _db.Settings.Select(c => new GetContactInfoDto
            {
                email = c.Email,
                phoneNumber = c.Phone,
            }).FirstOrDefaultAsync();
            return ResponseHelper.Success(contactInfo);
        }
        public async Task<BaseResponseDto<object>> AddContactUs(ContactUsDto model)
        {
            var contacUs = new ContactUs
            {
                Date = DateTime.UtcNow.AddHours(3),
                Email = model.email,
                UserName = model.user_Name,
                phoneCode = model.phoneCode,
                phoneNumber = model.phone,
                TitleMessage = model.titleMessage,
                Msg = model.message,
            };
            await _db.ContactUs.AddAsync(contacUs);
            await _db.SaveChangesAsync();
            return ResponseHelper.Success<object>(true, HelperMsg.creatMessage(model.lang, "تم الارسال بنجاح", "Send successfully"));
        }
        public async Task<BaseResponseDto<List<GetAllCityDto>>> GetAllCity(string lang)
        {
            var cities = await _db.City
                 .Where(c => c.IsActive)
                 .Select(c => new GetAllCityDto
                 {
                     id = c.Id,
                     name = c.ChangeLangName(lang)
                 }).ToListAsync();
            return ResponseHelper.Success(cities);
        }
        #region Provider Dashboard
        public async Task<BaseResponseDto<string>> GetProviderTermsAndCondition(string lang)
        {
            var conditionAndTermProviders = await _db.Settings
               .Select(c => c.ChangeLangCondtionProvider(lang))
               .FirstOrDefaultAsync();
            return ResponseHelper.Success(conditionAndTermProviders!);
        }
        public async Task<List<SelectListItem>> GetCitiesSelectList(int? cityId, string lang = "ar")
        {
            var cities = await _db.City.Where(c => c.IsActive)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.ChangeLangName(lang),
                     Selected = cityId != null ? c.Id == cityId.Value : false,
                 }).ToListAsync();
            return cities;
        }
        public async Task<List<RegoinsIndexViewModel>> GetAllRegionIncity(int cityId, string lang = "ar")
        {
            var regions = await _db.Distract.Where(c => c.IsActive && c.CityId == cityId)
                .Select(c => new RegoinsIndexViewModel
                {
                    Id = c.Id,
                    Name = c.ChangeLangName(lang),
                }).ToListAsync();
            return regions;
        }
        public async Task<List<SelectListItem>> GetAllRegionIncitySelectedList(int cityId, int? regionId, string lang = "ar")
        {
            var regions = await _db.Distract.Where(c => c.IsActive && c.CityId == cityId)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ChangeLangName(lang),
                    Selected = regionId != null ? c.Id == regionId.Value : false,
                }).ToListAsync();
            return regions;
        }
        public async Task<List<SelectListItem>> GetGenderType(int?selectedGender,string lang="ar")
        {
            var genders = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .Select(c=>new SelectListItem
                {
                   Value=((int)c).ToString(),
                   Text=Helper.GetGenderTypeText((int)c,lang),
                   Selected= selectedGender!=null ?(int)c== selectedGender:false,
                }).ToList();
                
            return genders;
        }
        public async Task<List<SelectListItem>> GetNationality(int? nationalitySelected,string lang = "ar")
        {
           var nationalities= await _db.Nationalities
                 .Where(c => c.IsActive).Select(c => new SelectListItem
                 {
                     Value= c.Id.ToString(),
                     Text=c.ChangeLangName(lang),
                     Selected= nationalitySelected!=null? nationalitySelected== c.Id:false,
                 }).ToListAsync();
            return nationalities;
        }
        #endregion

    }
}
