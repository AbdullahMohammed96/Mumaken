using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.MoreDto;
using Mumaken.Domain.DTO.SettingDto;
using Mumaken.Service.Api.Contract.More;

namespace Mumaken.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "MoreApi")]
    [ApiController]
    public class MoreController : ControllerBase
    {
        private readonly IMoreService _moreService;
        private readonly IHelper _helper;

        public MoreController(IMoreService moreService, IHelper helper)
        {
            _moreService = moreService;
            _helper = helper;
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.More.GetIntroScreen)]
        public async Task<IActionResult> GetIntroScreen(string lang = "ar")
        {
            var result = await _moreService.GetIntroScreen(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.More.GetAboutApp)]
        public async Task<IActionResult> GetAboutApp(string lang = "ar")
        {
            var result = await _moreService.GetAboutApp(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.More.GetClientAboutUs)]
        public async Task<IActionResult> GetClientAboutUs(string lang = "ar")
        {
            var result = await _moreService.GetClientAboutUs(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.More.GetClientConditionAndTerm)]
        public async Task<IActionResult> GetClientConditionAndTerm(string lang = "ar")
        {
            var result = await _moreService.GetClientTermsAndCondition(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.More.GetProvicyPolicy)]
        public async Task<IActionResult> GetProvicyPolicy(string lang = "ar")
        {
            var result = await _moreService.GetClientPrivacyPolicy(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.More.GetCommonQuestion)]
        public async Task<IActionResult> GetCommonQuestion(string lang = "ar")
        {
            var result = await _moreService.GetCommonQuestion(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.More.GetContactInfo)]
        public async Task<IActionResult> GetContactInfo(string lang = "ar")
        {
            var result = await _moreService.GetContactInfo(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpPost(ApiRoutes.More.AddContactUs)]
        public async Task<IActionResult> AddContactUs([FromForm]ContactUsDto model)
        {
            var result = await _moreService.AddContactUs(model);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.More.GetAllCity)]
        public async Task<IActionResult> GetAllCity(string lang)
        {
            var result = await _moreService.GetAllCity(lang);
            return StatusCode(result.ResponseCode, result);
        }

        [HttpPost(ApiRoutes.More.UploadImage)]
        [AllowAnonymous]
        public IActionResult UploadImage([FromForm] UploadImageDto model)
        {
            var result = _helper.Upload(model.image, model.fileName);
            return Ok(new { result });
        }
    }
}
