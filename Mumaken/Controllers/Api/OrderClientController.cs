using AAITHelper.Enums;
using AAITHelper;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.CouponDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.Copon;
using Mumaken.Service.Api.Contract.Logic;
using Mumaken.Domain.DTO.OrderDTO;
using Mumaken.Service.Api.Implementation.Auth;
using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Mumaken.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "OrderClient")]
    [ApiController]
    public class OrderClientController : ControllerBase
    {
        private readonly IOrderClient _OrderClient;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICouponService _couponService;
        public OrderClientController(ICurrentUserService currentUserService, IOrderClient OrderClient, ICouponService couponService, IOrderClient orderClient)
        {
            _currentUserService = currentUserService;
            _couponService = couponService;
            _OrderClient = orderClient;
        }
        [HttpPost(ApiRoutes.OrderApi.TestNotifcation)]
        [AllowAnonymous]
        public async Task<IActionResult> TestNotifcation(string userId)
        {
            var result = await _OrderClient.TestNotifcation(userId);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.AddNewOrder)]
        public async Task<IActionResult> AddNewOrder([FromForm] OrderAddDto model)
        {
            var result = await _OrderClient.AddNewOrder(model);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.GetOrderPaymentInfo)]
        public async Task<IActionResult> GetOrderPaymentInfo(int carId, string lang)
        {
            var result = await _OrderClient.GetOrderPaymentInfo(carId, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.UseCopon)]
        public async Task<IActionResult> UseCopon(double priceForDailyRental, int period, string coponCode, int carId,string lang = "ar")
        {
            var result = await _OrderClient.UseCopon(priceForDailyRental,period, coponCode, carId, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.GetOrderbyStatus)]
        public async Task<IActionResult> GetOrderbyStatus([FromForm] GetOrderByStatusDto model)
        {
            var result = await _OrderClient.GetOrderByStatus(model);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.GetOrderDetails)]
        public async Task<IActionResult> GetOrderDetails(int orderId,string lang="ar")
        {
            var result = await _OrderClient.GetOdrerDetails(orderId,lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.GetOrderPriceForCancleContact)]
        public async Task<IActionResult> GetOrderPriceForCancleContact(int orderId, string lang = "ar")
        {
            var result = await _OrderClient.GetOrderPriceForCancleContact(orderId, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.CancelContarct)]
        public async Task<IActionResult> CancelContarct(int orderId, string lang = "ar")
        {
            var result = await _OrderClient.CancelContarct(orderId, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.FinishContarct)]
        public async Task<IActionResult> FinishContarct(int orderId, string lang = "ar")
        {
            var result = await _OrderClient.FinishContarct(orderId, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.ConfirmReciptCar)]
        public async Task<IActionResult> ConfirmReciptCar(int orderId, string lang = "ar")
        {
            var result = await _OrderClient.ConfirmReciptCar(orderId, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.PayRentalPeriod)]
        public async Task<IActionResult> PayRentalPeriod(int orderId, int typePay,string lang = "ar")
        {
            var result = await _OrderClient.PayRentalPeriod(orderId, typePay, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.RenewOrderToAnotherPeriod)]
        public async Task<IActionResult> RenewOrderToAnotherPeriod(int orderId, int typePay, int rentalPeriod, string lang = "ar")
        {
            var result = await _OrderClient.RenewOrderToAnotherPeriod(orderId, typePay, rentalPeriod, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.CanceledOrder)]
        public async Task<IActionResult> CanceledOrder(int orderId, string reasonForCanceled, string lang="ar")
        {
            var result = await _OrderClient.CanceledOrder(orderId, reasonForCanceled, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.UseCoponInOrder)]
        public async Task<IActionResult> UseCoponInOrder(int orderId, string coponCode, string lang = "ar")
        {
            var result = await _OrderClient.UseCoponInOrder(orderId,coponCode, lang);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.OrderApi.PaymentOrder)]
        public async Task<IActionResult> PaymentOrder(int orderId, int buttontype, int typePay, int rentalPeriod = 0,string lang="ar")
        {
            var result = await _OrderClient.PaymentOrder(orderId, buttontype, typePay, rentalPeriod, lang);
            return StatusCode(result.ResponseCode, result);
        }
       
        [HttpPost(ApiRoutes.OrderApi.AddDeliveryCompanyToCurrentOrder)]
        public async Task<IActionResult> AddDeliveryCompanyToCurrentOrder([FromForm] AddDeliveryCompanyToCurrentOrderDto model)
        {
            var result = await _OrderClient.AddDeliveryCompaniesToCurentOrder(model);
            return StatusCode(result.ResponseCode, result);
        }
        // corn jop 
        [AllowAnonymous]
        [HttpGet(ApiRoutes.OrderApi.ChechEndRentalPeriod)]
        public async Task<IActionResult> ChechEndRentalPeriod()
        {
            var result = await _OrderClient.ChechEndRentalPeriod();
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.OrderApi.CancelOrderNotApproval)]
        public async Task<IActionResult> CancelOrderNotApproval()
        {
            var result = await _OrderClient.CancelOrderNotApproval();
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.OrderApi.FinishedOrderNotApprovalByRentalCompany)]
        public async Task<IActionResult> FinishedOrderNotApprovalByRentalCompany()
        {
            var result = await _OrderClient.FinishedOrderNotApprovalByRentalCompany();
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.OrderApi.FinishedOrderNotApprovalByDeliveryCompany)]
        public async Task<IActionResult> FinishedOrderNotApprovalByDeliveryCompany()
        {
            var result = await _OrderClient.FinishedOrderNotApprovalByDeliveryCompany();
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet(ApiRoutes.OrderApi.CalculateBreakingPeriodAndPrice)]
        public async Task<IActionResult> CalculateBreakingPeriodAndPrice()
        {
            var result = await _OrderClient.CalculateBreakingPeriodAndPrice();
            return Ok();
        }
    }
}
