using Mumaken.Domain.DTO;
using Mumaken.Domain.DTO.OrderDTO;
using Mumaken.Domain.ViewModel.AdminOrders;
using Mumaken.Domain.ViewModel.DeliveryCompanyOrder;
using Mumaken.Domain.ViewModel.FinancialAccount;
using Mumaken.Domain.ViewModel.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Api.Contract.Logic
{
    public interface IOrderProvider
    {
        Task<BaseResponseDto<List<GetRentalOrdersByStatusViewModel>>> GetRentalCompanyOrders(int orderStatus, string rentalCompanyId, string lang = "ar");
        Task<BaseResponseDto<object>> RefusedOrder(int orderId, string reasonForCanceled, string lang = "ar");
        Task<BaseResponseDto<object>> RefusedOrderByRentalCompany(int orderId, string reasonForCanceled, string lang = "ar");
        Task<BaseResponseDto<object>> AcceptOrderByAdministration(int orderId, string lang = "ar");
        Task<BaseResponseDto<object>> AcceptOrderByRentalCompany(AcceptOrderByRentalCompanyViewModel model);
        Task<BaseResponseDto<object>> AcceptOrderByDeliveryCompany(int orderId, string deliveryCompanyId,string lang = "ar");
        Task<BaseResponseDto<List<GetRentalCompanyReportViewModel>>> GetReports(string rentalCompanyId, string lang = "ar");
        Task<BaseResponseDto<ProviderWalletViewModel>> ProviderWallet(string rentalCompanyId);
        Task<BaseResponseDto<List<DeliveryCompanyOrdersViewModel>>> GetDeliveryCompanyOrders(int orderStatus, string deliveryCompanyId, string lang = "ar");
        Task<BaseResponseDto<object>> RefusedOrderByDeliveryCompany(int orderId, string deliverCompanyId, string refusedText, string lang = "ar");
        Task<BaseResponseDto<List<DeliveryCompanyReportViewModel>>> GetDeliveryCompanyReport(string deliveryCompanyId, string lang = "ar");
        Task<BaseResponseDto<object>> EnterDataForDeliverApp(EnterDataForDeliveryAppViewModel model);
        Task<BaseResponseDto<object>> ConfirmDataOfDeliveryApp(ComfirmDataOfDeliveryAppViewModel model);
        Task<BaseResponseDto<List<GetAllOrdersViewModel>>> GetAllOrders(int? orderStatus);
        Task<BaseResponseDto<List<GetRentalCompanyOrdersByAdminViewModel>>> GetRentalCompanyOrderByAdmin(int? filterType);
        Task<BaseResponseDto<List<GetDeliveryCompanyOrdersByAdminViewModel>>> GetDeliveyCompanyOrdersByAdmin(int? ordercase);
        Task<BaseResponseDto<OrderDetailsByAdminViewModel>> GetOrderDetailsByAdmin(int orderId);
        Task<BaseResponseDto<List<OrderCashFinancialAccountIndexViewModel>>> OrderCashConfirmBlancedRequest(bool confirm,int typepayOrder);
        Task<BaseResponseDto<List<OrderOnlineFinancialAccountIndexViewModel>>> OrderOnlineConfirmBlancedRequest(bool confirm, int typepayOrder);
        Task<BaseResponseDto<object>> ConfirmProviderbalance(int id, string lang = "ar");
        Task<BaseResponseDto<object>> CancelContactByAdministration(int orderId, string lang = "ar");
        Task<BaseResponseDto<object>> RequestToConfirmMySettle(RequestToConfirmMysettleViewModel model);
        Task<BaseResponseDto<object>> RequestToConfirmAdminSettle(RequestToConfirmAdminSettleViewModel model);
        Task<BaseResponseDto<object>> ConfirmRentalCompanyPaidOrder(int orderId, string lang = "ar");
    }
}
