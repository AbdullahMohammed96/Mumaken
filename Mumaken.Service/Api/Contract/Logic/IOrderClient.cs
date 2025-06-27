using Mumaken.Domain.Common.Helpers.ResponseHelper;
using Mumaken.Domain.DTO;
using Mumaken.Domain.DTO.OrderDTO;
using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Entities.SettingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Api.Contract.Logic
{
    public interface IOrderClient
    {
        Task<BaseResponseDto<object>> TestNotifcation(string userId);
        Task<BaseResponseDto<object>> AddNewOrder(OrderAddDto model);
        Task<BaseResponseDto<GetOrderPaymentInfoDto>> GetOrderPaymentInfo(int carId, string lang);
        Task<BaseResponseDto<CoponPriceModelDto>> UseCopon(double priceForDailyRental, int period, string coponCode, int carId, string lang = "ar");
        Task<BaseResponseDto<Pagination<OrderListDto>>> GetOrderByStatus(GetOrderByStatusDto model);
        Task<BaseResponseDto<OrderDetailsDto>> GetOdrerDetails(int orderId, string lang = "ar");
        Task<BaseResponseDto<OrderDetailsDto>> GetOdrerDetailsForCompanies(int orderId, string lang = "ar");
        Task<BaseResponseDto<object>> GetOrderPriceForCancleContact(int orderId, string lang = "ar");
        Task<BaseResponseDto<object>> CancelContarct(int orderId, string lang = "ar");
        Task<BaseResponseDto<object>> FinishContarct(int orderId, string lang = "ar");
        Task<BaseResponseDto<object>> ConfirmReciptCar(int orderId, string lang = "ar");
        Task<BaseResponseDto<object>> PaymentOrder(int orderId, int typePayemnt, int typePay,int rentalPeriod = 0, string lang = "ar");
        Task<BaseResponseDto<object>> CanceledOrder(int orderId, string reasonForCanceled,string lang = "ar");
        Task<BaseResponseDto<object>> PayRentalPeriod(int orderId, int typePay, string lang = "ar");
        Task<BaseResponseDto<object>> RenewOrderToAnotherPeriod(int orderId, int typePay, int rentalPeriod, string lang);
        Task<BaseResponseDto<CoponPriceModelDto>> UseCoponInOrder(int orderId, string coponCode, string lang = "ar");
   
     
        Task CalcateOrderPrice(double totalPriceWithoutDiscount, double dailyPriceForCar, int rentalPeriod, Order addOrder = null, string coponCode = "", RenewOrderdata renewOrderdata = null);
        Task<BaseResponseDto<object>> AddDeliveryCompaniesToCurentOrder(AddDeliveryCompanyToCurrentOrderDto model);

        FinancialAccount CreateFinancialAccount(Order order, int typePay, RenewOrderdata renewOrderdata = null);

        // Corn Jops
        Task<bool> ChechEndRentalPeriod();
        Task<bool> FinishedOrderNotApprovalByRentalCompany();
        Task<bool> FinishedOrderNotApprovalByDeliveryCompany();
        Task<bool> CancelOrderNotApproval();
        Task<bool> CalculateBreakingPeriodAndPrice();
    }
}
