using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.OrderDTO
{
    public class OrderDetailsDto
    {
        public InfoOrderDto orderInfo { get; set; }
        public CarInfoDto OrderCarInfo { get; set; }
        public List<DeliverCompanyOrderInfoDto> orderDeliveryCompanyInfo { get; set; }
        public OrderPaymentInfoDto paymentInfo { get; set; }
        public OrderUserInfoDto userInfo { get; set; }
        public RenewOrderInfoDto renewOrderInfo { get; set; }
    }
    public class InfoOrderDto
    {
        public int orderId { get; set; }
        public double price { get; set; }
        public string date { get; set; }
        public string orderDate { get; set; }
        public string orderCaseText { get; set; }
        public int orderCase { get; set; }
        public string? canceledText  { get; set; }
        public string? rejectionType { get; set; }
        public int orderStatus { get; set; }
        public string orderStatusText { get; set; }
        public int remaingDateToEndRental { get; set; }
        public string renewAndCancelContractText { get; set; }
        public bool isRenewToAnotherPeriod { get; set; }
    }
    public class CarInfoDto
    {
        public string rentalCompanyName { get; set; }
        public string carType { get; set; }
        public string carModel { get; set; }
        public string carCategory { get; set; }
        public string fileInsurancInformation { get; set; }
        public string insurancInformation { get; set; }
        public string carForm { get; set; }
        public int rentalPeriod { get; set; }
        public string dateStartPeriod { get; set; }
        public string? datePickupCar { get; set; }
        public string? timePickupCar { get; set; }
        public string?  locationPickupCar { get; set; }
        public string lat { get; set; }
        public string? lng { get; set; }
        public string? dateDeliveryCar { get; set; }
        public string? timeDeliveryCar { get; set; }
        public int newRentalPeriod { get; set; }  // مده الاستئجار الجديده
        public bool finishedRentalPeriod { get; set; }
    }
    public class DeliverCompanyOrderInfoDto
    {
        public string deliveryCompanyId { get; set; }
        public int orderCompnayCase { get; set; }
        public string companyName { get; set; }
        public string? loginData { get; set; } // The Data Delivery dashboard Add When Not Work in Delivery App
        public string? password { get; set; }
        public  string? reasonRefused  { get; set; }
        public string orderLoginData { get; set; } // the Data User Enter When Create Order When Work From Delivery App
        public bool isConfirmLoginData { get; set; } // the Confirm Data From Dashboard
        public bool isRefused { get; set; }
        public string  driverTypeInDliveryCompanyText { get; set; }  // انضمام ,تحديث
        public int  driverTypeInDliveryCompany { get; set; }  // انضمام ,تحديث
    }
    public class OrderPaymentInfoDto
    {
        public int newPeriod { get; set; }
        public double priceForDailyRental { get; set; }
        public double varPrice { get; set; }
        public double vatPercetage { get; set; }
        public double subTotal { get; set; }
        public double netPrice { get; set; }
        public double discountPrice { get; set; }
        public string patmentType { get; set; }
        public int FinishOrderOrderPeriod { get; set; }
        public double PriceFinishOrderOrderPeriod { get; set; }
        public int breakPeriod { get; set; }
        public double breakPrice { get; set; }
        public int dailyRentalCompanyPeriod { get; set; }
        public double dailyRentalCompanyPrice { get; set; }

    }
    public class OrderUserInfoDto
    {
        public string userId { get; set; }
        public string image { get; set; }
        public string user_Name { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string city { get; set; }
        public int age { get; set; }
        public int genderTypeId { get; set; }
        public int nationalityId { get; set; }
        public string genderType { get; set; }
        public string nationality { get; set; }
        public string identityNumber { get; set; }
        public string deliverLicenceImage { get; set; }
        public string indentityNumberImage { get; set; }
    }
    public class RenewOrderInfoDto
    {
        public int newPeriod { get; set; }
        public double netPrice { get; set; }
        public double subTotal { get; set; }
        public double discountPrice { get; set; }
        public double varPrice { get; set; }
        public double vatPercetage { get; set; }
        public int FinishOrderPeriod { get; set; }
        public double PriceFinishOrderPeriod { get; set; }
        public int breakPeriod { get; set; }
        public double breakPrice { get; set; }
        public int dailyRentalCompanyPeriod { get; set; }
        public double dailyRentalCompanyPrice { get; set; }
    }
}
