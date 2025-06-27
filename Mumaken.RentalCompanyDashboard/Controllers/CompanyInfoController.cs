using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.ViewModel.Provider;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.More;
using Mumaken.Service.Api.Implementation.Auth;
using NToastNotify;

namespace Mumaken.RentalCompanyDashboard.Controllers
{
    [Authorize]
    public class CompanyInfoController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly ICurrentUserService _currentUser;
        private readonly IMoreService _moreServices;
        private readonly IToastNotification _toastNotification;
        private readonly IAccountService _accountService;
        public CompanyInfoController(IUserServices userServices, ICurrentUserService currentUser, IMoreService moreServices, IToastNotification toastNotification, IAccountService accountService)
        {
            _userServices = userServices;
            _currentUser = currentUser;
            _moreServices = moreServices;
            _toastNotification = toastNotification;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var lang = Helper.GetLanguage();
            var providerInfo = await _userServices.GetProviderInfo(_currentUser.user.Id, lang, "");
            ViewData["Cites"] = await _moreServices.GetCitiesSelectList(providerInfo.cityId, lang);
            ViewData["Regions"] = await _moreServices.GetAllRegionIncitySelectedList(providerInfo.cityId, providerInfo.distractId,lang);
            return View(providerInfo);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfileInfo(UpadateProviderViewModel model)
        {
           var updatePrviderInfoResult=await _userServices.UpdateProvider(model);

            if (updatePrviderInfoResult.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(updatePrviderInfoResult.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> ReSendCode()
        {
            var result = await _accountService
                 .ResendCode(new ResendCodeAddDto { userId = _currentUser.user.Id, lang = Helper.GetLanguage() });
            if (result.IsSuccess)
            {
                return Json(new { key = 1, msg = result.Message });
            }
            return Json(new { key = 0, msg = result.Message });
        }
    }
}
