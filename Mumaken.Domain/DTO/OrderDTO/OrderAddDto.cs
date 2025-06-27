using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Mumaken.Domain.DTO.OrderDTO
{
    /// <summary>
    /// I Split Add Order Base on Ui Screen
    /// </summary>
    public class OrderAddDto
    {
        public OrderAddUserInfoDto userInfoDto { get; set; }
        public OrderAddCarInfoDto carInfo { get; set; }
        public OrderAddDeliverCompanyInfoDto deliverCompanyInfo { get; set; }
        public string lang { get; set; }
    }

    /// <summary>
    /// Order User Info 
    /// </summary>
    public class OrderAddUserInfoDto
    {
        public int genderType { get; set; }
        public int notionalityId { get; set; }
        public int age { get; set; }
        public string identityNumber { get; set; }
        public IFormFile identityImage { get; set; }
        public IFormFile drivingLicenseImage { get; set; }
    }
    public class OrderAddCarInfoDto
    {
        public int carId { get; set; }
        public int rentalPeriod { get; set; }  //مده التاخير
        public DateTime dateRentalPeriod { get; set; }  //تاريخ بدايه الاستجار
        public int TypePay { get; set; }
        public string? CoponCode { get; set; }

    }
    //DliverCompany Information
    public class OrderAddDeliverCompanyInfoDto
    {
        public List<CompanyInfoDto> workedCompany { get; set; }  // شركات التوصيل التي تعمل معها فعلا
        public List<string> newComanyWantedWorkId { get; set; }  // شركات التوصيل التي تريد ان تعمل معها
    }
    public class CompanyInfoDto
    {
        public string deliverComapnyId { get; set; }
        public string loginData { get; set; }
    }
}
