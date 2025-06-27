using AAITHelper;
using AAITHelper.Enums;
using AAITHelper.ModelHelper;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Notification;
using Mumaken.Domain.ViewModel.Settings;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.DashBoard.Contract.NotificationInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Mumaken.Domain.Common.Helpers.DataTablePaginationServer;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.UserIndexDto;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Notifications)]
    public class SendNotificationController : Controller
    {
        private readonly INotificationServices _notificationServices;
        public SendNotificationController(INotificationServices notificationServices = null)
        {
            _notificationServices = notificationServices;
        }

        public async Task<IActionResult> Index()
        {
            var UserNotifies = await _notificationServices.GetHistoryNotify();

            return View(UserNotifies);

        }
        public async Task<IActionResult> GetAllNotifcation()
        {
            var allNotification = await _notificationServices.GetAllNotification();
            return View(allNotification);

        }
        [HttpPost]
        public async Task<IActionResult> GetAllNotifcationForExcel()
        {
            var allNotification = await _notificationServices.GetAllNotification();
            return Json(allNotification);

        }
        public IActionResult SendNotify()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _notificationServices.GetUsers();

            return Ok(new { key = 1, users });
        }

        [HttpGet]
        public async Task<IActionResult> GetDeliveryCompany()
        {
            var delegets = await _notificationServices.GetDeliveryCompany();

            return Ok(new { key = 1, delegets });
        }
        [HttpGet]
        public async Task<IActionResult> GetRentalCompany()
        {
            var delegets = await _notificationServices.GetRentalCompany();

            return Ok(new { key = 1, delegets });
        }

        [HttpPost]
        public async Task<IActionResult> Send(string msg, string employees, string deliveryCompanyes, string rentalCompanyes)
        {

            await _notificationServices.Send(msg, employees, deliveryCompanyes, rentalCompanyes);

            return Ok(new { redirectToUrl = Url.Action("Index", "SendNotification") });
        }
    }
}