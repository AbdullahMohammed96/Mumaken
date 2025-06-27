using Mapster;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Auth;
using Mumaken.Domain.ViewModel.Provider;
using Mumaken.Service.Api.Contract.Auth;
using NToastNotify;

namespace Mumaken.DeliverCompanyDashboard.Controllers
{
    public class DAuthController : Controller
    {
        private readonly IAccountService _accountService;

        private readonly IToastNotification _toastNotification;
        public DAuthController(IAccountService accountService, IToastNotification toastNotification)
        {
            _accountService = accountService;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> ActiveCode(string userId)
        {
            ViewBag.userId = userId;
            ViewBag.lang = Helper.GetLanguage();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ActiveCode(ConfirmCodeAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var confirmCodemodel = model.Adapt<ConfirmCodeAddDto>();
                var checkRegisterCodeResult = await _accountService.ConfirmCodeRegister(confirmCodemodel);
                if (checkRegisterCodeResult.IsSuccess)
                {
                    return RedirectToAction("Index", "Orders",new { orderStatus=(int)OrderStutes.NewOrder });
                }
                else
                {
                    _toastNotification.AddErrorToastMessage(checkRegisterCodeResult.Message, new ToastrOptions { Title = "" });
                    ViewBag.userId = model.userId;
                    ViewBag.lang = Helper.GetLanguage();
                    return View();
                }
            }
            ViewBag.userId = model.userId;
            ViewBag.lang = Helper.GetLanguage();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ForegetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForegetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lang = Helper.GetLanguage();
                var result = await _accountService.ForgetProviderPassword(
                    new ForgetPasswordViewModel { phone = model.phone, lang = lang, phoneCode = model.phoneCode });
                if (result.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage(lang == "ar" ? "تم إرسال كود التحقق" : "Activation code has been sent", new ToastrOptions { Title = "" });
                    return RedirectToAction("CheckCode", new { userId = result.Data.userId });

                }
                ModelState.AddModelError("phone", result.Message);
                return View(model);

            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> CheckCode(string userId)
        {
            ViewBag.userId = userId;
            ViewBag.lang = Helper.GetLanguage();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CheckCode(CheckCodeViewModel model)
        {
            var lang = Helper.GetLanguage();
            var result = await _accountService.CheckCode(model.UserId, model.Code, lang);
            if (result.IsSuccess)
            {
                return RedirectToAction("ResetPassword", new { userId = model.UserId });
            }
            else
            {
                ViewBag.userId = model.UserId;
                ViewBag.lang = lang;
                //ModelState.AddModelError("Code", result.Message);
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "" });
                return View();
            }


        }
        public ActionResult ResetPassword(string userId)
        {
            ViewBag.userId = userId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordByCodeViewModel model)
        {

            var result = await _accountService.ChangePasswordByCode(new ChangePasswordByCodeDto { newPassword = model.newPassword, userId = model.userId, lang = model.lang });
            if (result.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(model.lang == "ar" ? "تم التغيير  بنجاح " : "The change was made successfully", new ToastrOptions { Title = "" });
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "" });
            ViewBag.userId = model.userId;
            ModelState.AddModelError("newPassword", result.Message);
            return View();
        }
        public async Task<IActionResult> ResendCode(string id, int type = 1)
        {
            var resendCodeResult = await _accountService
                .ResendCode(new ResendCodeAddDto { lang = Helper.GetLanguage(), userId = id });
            if (resendCodeResult.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(resendCodeResult.Message, new ToastrOptions { Title = "" });
                if (type == 1)
                    return RedirectToAction(nameof(CheckCode), new { userId = id });
                return RedirectToAction(nameof(ActiveCode), new { userId = id });
            }
            _toastNotification.AddErrorToastMessage(resendCodeResult.Message, new ToastrOptions { Title = "" });
            return RedirectToAction(nameof(ActiveCode), new { userId = id });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePhoneNumber(ChangePhoneNumberViewModel model)
        {
            model.lang = Helper.GetLanguage();
            var changeProviderPhone = await _accountService.ChangeProviderPhoneNumber(model);
            if (changeProviderPhone.IsSuccess)
            {
                return Json(new { key = 1, data = changeProviderPhone.Data, msg = changeProviderPhone.Message });
            }

            return Json(new { key = 0, msg = changeProviderPhone.Message });
        }
        public async Task<IActionResult> CheckChangePhoneCode(CheckChangePhoneCodeViewModel model)
        {
            model.lang = Helper.GetLanguage();
            var changeProviderPhone = await _accountService.CheckChangePhoneCode(model);

            if (changeProviderPhone.IsSuccess)
                return Json(new { Key = 1, msg = changeProviderPhone.Message });

            return Json(new { Key = 0, msg = changeProviderPhone.Message });

        }
        public async Task<IActionResult> ChangePassword(ChangeProviderPasswordViewModel model)
        {
            var changePasswordResult = await _accountService.ChangePassword(new ChangePasswordDto
            {
                lang = Helper.GetLanguage(),
                newPassword = model.NewPassword,
                oldPassword = model.CurrentPassword,
                userId = model.UserId
            });
            if (changePasswordResult.IsSuccess)
            {
                return Json(new { Key = 1, msg = changePasswordResult.Message });
            }
            return Json(new { Key = 0, msg = changePasswordResult.Message });
        }
    }
}
