using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Orders;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.Logic;
using NToastNotify;

namespace Mumaken.DeliverCompanyDashboard.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderProvider _orderProvider;
        private readonly IOrderClient _orderClient;
        private readonly ICurrentUserService _currentUser;
        private readonly IToastNotification _toastNotification;

        public OrdersController(ICurrentUserService currentUser, IOrderProvider orderProvider, IOrderClient orderClient, IToastNotification toastNotification)
        {
            _currentUser = currentUser;
            _orderProvider = orderProvider;
            _orderClient = orderClient;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(int orderStatus = 1)
        {
            orderStatus = orderStatus == 0 ? 1 : orderStatus;  // Health Check 

            var currentUser = _currentUser.user;
            ViewBag.orderStatus = orderStatus;
            if (currentUser!=null&& currentUser.IsAppravel)
            {
                var result = await _orderProvider.GetDeliveryCompanyOrders(orderStatus, currentUser.Id, Helper.GetLanguage());
                ViewBag.IsAppravel = true;
                return View(result.Data);
            }
            ViewBag.IsAppravel = false;
            return View();
        }
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var currentUser = _currentUser.user?.Id;
            ViewBag.currentUser = currentUser;
            var orderDetails = await _orderClient.GetOdrerDetailsForCompanies(orderId, Helper.GetLanguage());
            return View(orderDetails.Data);
        }
        public async Task<IActionResult> AcceptOrder(int orderId)
        {
            var result = await _orderProvider.AcceptOrderByDeliveryCompany(orderId, _currentUser.user.Id, Helper.GetLanguage());
            _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });

            return RedirectToAction(nameof(OrderDetails), new { orderId = orderId });
        }
        public async Task<IActionResult> RefusedOrder(int orderId, string refusedText)
        {
            var result = await _orderProvider.RefusedOrderByDeliveryCompany(orderId, _currentUser.user.Id, refusedText, Helper.GetLanguage());
            if (result.IsSuccess)
            {
                return Json(new { key = 1, msg = result.Message });

            }
            return Json(new { key = 0, msg = result.Message });
        }
        public async Task<IActionResult> GetReports()
        {
            var reports = await _orderProvider.GetDeliveryCompanyReport(_currentUser.user.Id, Helper.GetLanguage());
            return View(reports.Data);
        }
        public async Task<IActionResult> EnterDataForDeliverApp(EnterDataForDeliveryAppViewModel model)
        {
            model.DeliveryCompantId = _currentUser.user?.Id;
            model.lang=Helper.GetLanguage();
            var result = await _orderProvider.EnterDataForDeliverApp(model);
            return Json(new { key = 1, msg = result.Message });
        }
        public async Task<IActionResult> ConfirmDataOfDeliverApp(ComfirmDataOfDeliveryAppViewModel model)
        {
            model.lang= Helper.GetLanguage();   
            model.DeliveyCompanyId = _currentUser.user?.Id;  
            var result = await _orderProvider.ConfirmDataOfDeliveryApp(model);
            return Json(new { key = 1, msg = result.Message });
        }

    }
}
