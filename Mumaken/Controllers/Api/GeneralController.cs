using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.GeneralDTO;
using Mumaken.Domain.Enums;
using Mumaken.Service.Api.Contract.General;

namespace Mumaken.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "GeneralApi")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly IGeneralService _generalService;
        public GeneralController(IGeneralService generalService)
        {
            _generalService = generalService;
        }
        [HttpGet(ApiRoutes.General.HomePage)]
        [AllowAnonymous]
        public async Task<IActionResult> HomePage(string lang = "ar")
        {
            var result = await _generalService.GetHomePage(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.General.GetOrderSplash)]
        public async Task<IActionResult> GetOrderSplash(string lang = "ar")
        {
            var result = await _generalService.GetOrderCycleSplash(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpGet(ApiRoutes.General.GetNationalities)]
        public async Task<IActionResult> GetNationalities(string lang = "ar")
        {
            var result = await _generalService.GetNationalities(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.General.GetCars)]
        public async Task<IActionResult> GetCars(int pageNumber, int pageSize, string lang = "ar")
        {
            var result = await _generalService.GetCars(pageNumber, pageSize, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpGet(ApiRoutes.General.GetAllCarCatrgory)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCarCatrgory(string lang = "ar")
        {
            var result = await _generalService.GetAllCarCatrgory(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpGet(ApiRoutes.General.GetAllCarType)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCarType(string lang = "ar")
        {
            var result = await _generalService.GetAllCarType(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.General.GetAllCarModelWithCarType)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCarModelWithCarType([FromForm]GetAllCarModelWithTypeDto model)
        {
            var result = await _generalService.GetAllCarModelWithCarType(model);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpGet(ApiRoutes.General.GetAllCities)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCities(string lang = "ar")
        {
            var result = await _generalService.GetAllCities(lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpGet(ApiRoutes.General.GetAllDeliveryCompanies)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDeliveryCompany(int pageNumber,int pageSize,int? orderId, string lang = "ar")
        {
            var result = await _generalService.GetDeliveryCompany(pageNumber, pageSize, orderId, lang) ;
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.General.Filter)]
        public async Task<IActionResult> Filter([FromForm]FilterRequestDto model)
        {
            var result = await _generalService.FilterCar(model);
            return StatusCode(result.ResponseCode, result);
        }

    }
}
