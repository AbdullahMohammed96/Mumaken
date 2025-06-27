using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Service.Api.Contract.Logic;
using Mumaken.Service.DashBoard.Contract.ContactUsInterfaces;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Orders)]
    public class OrdersController : Controller
    {
        private readonly IOrderProvider _orderProvider;
        private readonly IOrderClient _orderClient;
        private readonly IToastNotification _toastNotification;
        public OrdersController(IOrderProvider orderProvider, IOrderClient orderClient, IToastNotification toastNotification)
        {
            _orderProvider = orderProvider;
            _orderClient = orderClient;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(int? status)
        {
            var result = await _orderProvider.GetAllOrders(status);
            var selectlist = Helper.OrderStatusSelectList(Helper.GetLanguage());
            ViewBag.orderTypes = new SelectList(selectlist, "Value", "Text", status);
            return View(result.Data);
        }
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var result = await _orderProvider.GetOrderDetailsByAdmin(orderId);
            return result.Data is null ? NotFound() : View(result.Data);
        }
        public async Task<IActionResult> GetRentalCompanyOrders(int? ordercase = 1)
        {
            var result = await _orderProvider.GetRentalCompanyOrderByAdmin(ordercase);
            var selectlist = Helper.OrderCaseSelectList(Helper.GetLanguage());
            ViewBag.orderTypes = new SelectList(selectlist, "Value", "Text", ordercase);
            ViewBag.ordercase = ordercase;
            return View(result.Data);
        }
        public async Task<IActionResult> GetDeliveryCompanyOrders(int? ordercase = 3)
        {

            var result = await _orderProvider.GetDeliveyCompanyOrdersByAdmin(ordercase);
            var selectlist = Helper.OrderDeliveyCompanyCaseSelectList(Helper.GetLanguage());
            ViewBag.orderTypes = new SelectList(selectlist, "Value", "Text", ordercase);
            ViewBag.ordercase = ordercase;
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int id)
        {
            var result = await _orderProvider.AcceptOrderByAdministration(id, Helper.GetLanguage());
            return Json(new { key = 1, msg = result.Message });
            //return RedirectToAction(nameof(GetRentalCompanyOrders), new { ordercase = (int)OrderCase.WaitToAcceptDeliverCompany });
        }
        public async Task<IActionResult> RefusedOrder(int id, string reasonText = "")
        {
            var result = await _orderProvider.RefusedOrder(id, reasonText, Helper.GetLanguage());
            if (result.IsSuccess)
            {
                return Json(new { key = 1, msg = result.Message });
            }
            return Json(new { key = 0, msg = result.Message });
        }
        public async Task<IActionResult> CancelContact(int id)
        {
            var result = await _orderProvider.CancelContactByAdministration(id, Helper.GetLanguage());
            return Json(new { key = 1, msg = result.Message });
        }
    }
}
