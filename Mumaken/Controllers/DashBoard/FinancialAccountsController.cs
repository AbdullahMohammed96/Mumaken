using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Service.Api.Contract.Logic;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.FinancialAccounts)]
    public class FinancialAccountsController : Controller
    {
        private readonly IOrderProvider _orderProvider;

        public FinancialAccountsController(IOrderProvider orderProvider)
        {
            _orderProvider = orderProvider;
        }
        //الطلبات الكاش التي لم يتم تسويتها

        public async Task<IActionResult> OrderCashFinancialAccountsNotConfirmed()
        {
            var result = await _orderProvider.OrderCashConfirmBlancedRequest(false, (int)PaymentMethod.Cash);
            return View(result.Data);
        }
        public async Task<IActionResult> OrderCashFinancialAccountsConfirmed()
        {
            var result = await _orderProvider.OrderCashConfirmBlancedRequest(true, (int)PaymentMethod.Cash);
            return View(result.Data);
        }
        //الطلبات الاونلاين التي لم يتم تسويتها
        public async Task<IActionResult> OrderOnlineFinancialAccountsNotConfirmed()
        {
            var result = await _orderProvider.OrderOnlineConfirmBlancedRequest(false, (int)PaymentMethod.Online);
            return View(result.Data);
        }
        public async Task<IActionResult> OrderOnlineFinancialAccountssConfirmed()
        {
            var result = await _orderProvider.OrderOnlineConfirmBlancedRequest(true, (int)PaymentMethod.Online);
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmBalance(int id)
        {
            var result=await _orderProvider.ConfirmProviderbalance(id,Helper.GetLanguage());
            return Json(new { key=1 });
        }
    }
}
