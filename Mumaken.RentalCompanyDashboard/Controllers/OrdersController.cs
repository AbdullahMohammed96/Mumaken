using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Orders;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.Logic;
using Nancy;
using NToastNotify;

namespace Mumaken.RentalCompanyDashboard.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderProvider _orderProvider;
        private readonly IOrderClient _orderClient;
        private readonly ICurrentUserService _currentUser;
        private readonly IToastNotification _toastNotification;
        public OrdersController(IOrderProvider orderProvider, ICurrentUserService currentUser, IOrderClient orderClient, IToastNotification toastNotification)
        {
            _orderProvider = orderProvider;
            _currentUser = currentUser;
            _orderClient = orderClient;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(int ordercase = 1)
        {
            var orders = await _orderProvider.GetRentalCompanyOrders(ordercase, _currentUser?.user?.Id, Helper.GetLanguage());
            ViewBag.ordercase = ordercase;
            if (orders.IsSuccess)
                return View(orders.Data);

            ViewBag.message = orders.Message;
            ViewBag.isSuccess = orders.IsSuccess;
            return View();

        }

        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var lang=Helper.GetLanguage();
            var orderDetails = await _orderClient.GetOdrerDetailsForCompanies(orderId, lang);
            orderDetails.Data.orderInfo.orderCaseText = Helper.OrderCaseRentalCompanyText(orderDetails.Data.orderInfo.orderCase, lang);
            return View(orderDetails.Data);
        }
        [HttpPost]
        public async Task<IActionResult> RefusedOrder(int refusedOrderId, string refusedReason)
        {
            var refusedOrderReason = await _orderProvider.RefusedOrderByRentalCompany(refusedOrderId, refusedReason, Helper.GetLanguage());
            _toastNotification.AddSuccessToastMessage(refusedOrderReason.Message, new ToastrOptions { Title = "" });
            return RedirectToAction(nameof(Index), new { status = (int)OrderCase.WaitToAcceptRentalCompany });
        }
        public async Task<IActionResult> GetReports()
        {
            var reports = await _orderProvider.GetReports(_currentUser.user.Id, Helper.GetLanguage());
            return View(reports.Data);
        }
        public async Task<IActionResult> ProviderWalet()
        {
            var providerWallet = await _orderProvider.ProviderWallet(_currentUser.user.Id);
            var banks = providerWallet.Data.AdminBanks.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.BankName,
            }).ToList();
            ViewBag.banks=banks;
            return View(providerWallet.Data);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptOrder(AcceptOrderByRentalCompanyViewModel accpetedRentalModel)
        {
            accpetedRentalModel.lang=Helper.GetLanguage();
            var result = await _orderProvider.AcceptOrderByRentalCompany(accpetedRentalModel);
            if (result.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });

                return RedirectToAction(nameof(Index), new { status =(int)OrderCase.Accepted});
            }
            else
            {
                _toastNotification.AddErrorToastMessage(result.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(OrderDetails), new { orderId = accpetedRentalModel.OrderId });
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> RequestToConfirmMySettle(RequestToConfirmMysettleViewModel model)
        {
            model.CompanyId = _currentUser.user.Id;
            model.lang=Helper.GetLanguage();
            var result=await _orderProvider.RequestToConfirmMySettle(model);
            if (result.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(ProviderWalet));
            }
            return Json(new {key=0});
        }
        [HttpPost]
        public async Task<IActionResult> RequestToConfirAdminSettle(RequestToConfirmAdminSettleViewModel model)
        {
            model.CompanyId = _currentUser.user.Id;
            model.lang = Helper.GetLanguage();
            var result = await _orderProvider.RequestToConfirmAdminSettle(model);
            if (result.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(ProviderWalet));
            }
            return Json(new { key = 0 });
        }
        public async Task<IActionResult> ConfirmReceiptCar(int orderId)
        {
            var lang=Helper.GetLanguage();

           var resultConfirmReciptcar =await _orderClient.ConfirmReciptCar(orderId,lang);
            if (resultConfirmReciptcar.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(resultConfirmReciptcar.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(OrderDetails),new { orderId = orderId });
            }
            return Json(new { key = 0 });
        }
        public async Task<IActionResult> ConfirmPaymentOrder(int orderId)
        {
            var lang = Helper.GetLanguage();

            var result = await _orderProvider.ConfirmRentalCompanyPaidOrder(orderId, lang);
            if (result.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(OrderDetails), new { orderId = orderId });
            }
            return Json(new { key = 0 });
        }
    }
}
