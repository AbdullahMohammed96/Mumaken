using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.DTO.MoreDto;
using Mumaken.Domain.ViewModel.ContactUs;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.More;
using NToastNotify;

namespace Mumaken.RentalCompanyDashboard.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly IMoreService _moreServices;
        private readonly ICurrentUserService _currentUser;
        private readonly IToastNotification _toastNotification;
        public SettingController(IMoreService moreServices, IToastNotification toastNotification, ICurrentUserService currentUser)
        {
            _moreServices = moreServices;
            _toastNotification = toastNotification;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> AboutUs()
        {
            var lang = Helper.GetLanguage() ?? "ar";
            var aboutUs = await _moreServices.GetAboutAppProvider(lang);
            ViewBag.aboutUs = aboutUs.Data;
            ViewBag.lang = lang;
            return View();
        }
        public async Task<IActionResult> GetCommonQuestion()
        {
            var lang = Helper.GetLanguage() ?? "ar";
            var commonQuestion = await _moreServices.GetCommonQuestion(lang);
            return View(commonQuestion.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetConditionAndTerms()
        {
            var lang = Helper.GetLanguage() ?? "ar";
            var conditionAndTerms = await _moreServices.GetProviderTermsAndCondition(lang);
            ViewBag.conditionAndTerms = conditionAndTerms.Data;
            ViewBag.lang = lang;
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetConditionAndTermsForAllUser()
        {
            var lang = Helper.GetLanguage() ?? "ar";
            var conditionAndTerms = await _moreServices.GetProviderTermsAndCondition(lang);
            ViewBag.conditionAndTerms = conditionAndTerms.Data;
            ViewBag.lang = lang;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetContactus()
        {
            var lang = Helper.GetLanguage() ?? "ar";
            var infoContact = await _moreServices.GetContactInfo(lang);
            ViewBag.infoContact = infoContact.Data;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddContactUs(AddContactUsViewModel model)
        {
            var currentUser=_currentUser.user;
            var lang = Helper.GetLanguage()??"ar";
            if (ModelState.IsValid)
            {
                var result = await _moreServices.AddContactUs(new ContactUsDto
                {
                    message = model.Message,
                    titleMessage = model.MessageSubject,
                    phone = currentUser.PhoneNumber,
                    phoneCode = currentUser.PhoneCode,
                    user_Name = currentUser.user_Name,
                    lang = lang,
                });
                if (result.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                    return RedirectToAction("GetContactus", "Setting");
                }
            }
            var infoContact = await _moreServices.GetContactInfo(lang);
            ViewBag.infoContact = infoContact.Data;
            return View(model);

        }
    }
}
