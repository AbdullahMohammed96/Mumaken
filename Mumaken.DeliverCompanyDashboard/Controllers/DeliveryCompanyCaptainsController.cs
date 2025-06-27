using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.ViewModel.Auth;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.More;
using NToastNotify;

namespace Mumaken.DeliverCompanyDashboard.Controllers
{
    public class DeliveryCompanyCaptainsController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly ICurrentUserService _currentUser;
        private readonly IMoreService _moreServices;
        private readonly IToastNotification _toastNotification;
        private readonly IAccountService _accountService;

        public DeliveryCompanyCaptainsController(IUserServices userServices, ICurrentUserService currentUser, IMoreService moreServices, IToastNotification toastNotification, IAccountService accountService)
        {
            _userServices = userServices;
            _currentUser = currentUser;
            _moreServices = moreServices;
            _toastNotification = toastNotification;
            _accountService = accountService;
        }

        public async Task<IActionResult> GetCompanyCaptains()
        {
            var captains = await _accountService.GetDeliveryCompanyCaptains(_currentUser.user?.Id, Helper.GetLanguage());
            return View(captains.Data);
        }
        public async Task<IActionResult> AddCaptain()
        {
            var lang = Helper.GetLanguage();
            ViewBag.Lang = lang;
            ViewBag.deliverCompanyId = _currentUser.user?.Id;
            ViewData["Cites"] = await _moreServices.GetCitiesSelectList(null, lang);
            ViewData["Nationalities"] = await _moreServices.GetNationality(null, lang);
            ViewData["Genders"] = await _moreServices.GetGenderType(null, lang);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCaptain(AddCapationViewModel model)
        {
            var lang= Helper.GetLanguage();
            model.lang = lang;
            if (ModelState.IsValid)
            {
                var result = await _accountService.AddCapatin(model);
                if (result.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                    return RedirectToAction(nameof(GetCompanyCaptains));
                }
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "" });
            }
            ViewBag.Lang = lang;
            ViewBag.deliverCompanyId = _currentUser.user?.Id;
            ViewData["Cites"] = await _moreServices.GetCitiesSelectList(null, lang);
            ViewData["Nationalities"] = await _moreServices.GetNationality(null, lang);
            ViewData["Genders"] = await _moreServices.GetGenderType(null, lang);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCaption(string id)
        {
            var lang = Helper.GetLanguage();
            var result = await _accountService.GetCaptionToUpdate(id, lang);
            ViewBag.Lang = lang;
            ViewData["Cites"] = await _moreServices.GetCitiesSelectList(result.Data.CityId, lang);
            ViewData["Nationalities"] = await _moreServices.GetNationality(result.Data.Nationality, lang);
            ViewData["Genders"] = await _moreServices.GetGenderType(result.Data.GenderType, lang);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCaption(EditCaptainViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.UpdateCaption(model);
                if (result.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                    return RedirectToAction(nameof(GetCompanyCaptains));
                }
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(UpdateCaption), new { id = model.Id });
            }
            return RedirectToAction(nameof(UpdateCaption), new { id = model.Id });
        }
        public async Task<IActionResult> GetCaptainDetails(string id)
        {
            var captainDetails = await _accountService.GetCaptainDetails(id, Helper.GetLanguage());
            return View(captainDetails.Data);
        }
        public async Task<IActionResult> deleteCaptain(string id)
        {
            var result = await _accountService.DeleteCapation(id, Helper.GetLanguage());
            if (result.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(GetCompanyCaptains));
            }
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "" });
            return RedirectToAction(nameof(GetCaptainDetails), new { id = id });
        }
    }
}
