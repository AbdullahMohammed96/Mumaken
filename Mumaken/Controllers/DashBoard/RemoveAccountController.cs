using Microsoft.AspNetCore.Mvc;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Implementation.Auth;

namespace Mumaken.Controllers.DashBoard
{
    public class RemoveAccountController : Controller
    {
        private readonly IAccountService _accountService;

        public RemoveAccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendActivationCode(string phone)
        {
            // Your logic to send activation code
            var user = await _accountService.GetUserUsingPhone(phone,"+966");
            if (user == null)
            {
                return Json(new { success = false });

            }
            int code = await _accountService.GenerateCode(1234);

            var sendSmsResult = await _accountService.SendSms(code, phone);
            var Result = await _accountService.UpdateCode(phone, "+966",code);


            return Json(new { success = true });

        }
        [HttpPost]
        public async Task<IActionResult> CompareCodes(string inputCode, string phone)
        {
            // Your logic to compare the input code with the stored code
            var user = await _accountService.GetUserUsingPhone(phone, "+966");


            if (user == null)
            {
                return Json(new { success = false });
            }

            if (user.Code.ToString() == inputCode)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        [HttpPost]
        public async Task<IActionResult> RemoveAccount(string phone)
        {
            // Your logic to remove the account
            var user = await _accountService.GetUserUsingPhone(phone, "+966");

            if (user != null)
            {
                var isremoved = await _accountService.RemoveAccount(user.Id,"ar");

                return Json(new { success = true });
            }
            else
            {

                return Json(new { success = false });

            }
        }

    }
}
