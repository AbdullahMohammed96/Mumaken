using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.RentalCompanyDashboard.Models;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.More;
using NToastNotify;
using System.Diagnostics;

namespace Mumaken.RentalCompanyDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMoreService _moreServices;
        private readonly IUserServices _userService;
        private readonly ICurrentUserService _currentUser;
        private readonly IToastNotification _toastNotification;
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        public HomeController(ILogger<HomeController> logger, IMoreService moreServices, IUserServices userService, ICurrentUserService currentUser, IToastNotification toastNotification, SignInManager<ApplicationDbUser> signInManager)
        {
            _logger = logger;
            _moreServices = moreServices;
            _userService = userService;
            _currentUser = currentUser;
            _toastNotification = toastNotification;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
        public async Task<IActionResult> GetAllNotification()
        {
            var notifies = await _userService.ListOfNotifyProvider(_currentUser.user.Id, Helper.GetLanguage());
            return View(notifies.Data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAllNotification()
        {
            var result = await _userService.DeleteAllDelgateNotify(_currentUser.user.Id, Helper.GetLanguage());
            return Json(new { key = 1, msg = result.Message });
        }
        public async Task<IActionResult> DeleteNotificationById(int notifyId)
        {
            var result = await _userService.DeleteDelgateNotifyById(notifyId, Helper.GetLanguage());
            return Json(new { key = 1, msg = result.Message });
        }
        [AllowAnonymous]
        public async Task<IActionResult> GetRegionsByCityId(int cityId)
        {
            var regions = await _moreServices.GetAllRegionIncity(cityId, Helper.GetLanguage());
            return Ok(regions);
        }
        public IActionResult EmptyAction()
        {
            return Ok();
        }
    }
}
