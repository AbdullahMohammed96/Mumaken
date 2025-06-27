using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.AdminOrders
{
    public class OrderDetailsByAdminViewModel
    {
        public int OrderId { get; set; }
        public int RentalPeriod { get; set; }  //مدة الاستئجار
        public string DateRentalPeriod { get; set; }  //تاريخ بداية الاستئجار
        public string Date { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string DriverLicenseImage { get; set; }
        public string IdentityImage { get; set; }
        public string IdentityNumber { get; set; }
        public string CarModel { get; set; }
        public string CarCategory { get; set; }
        public string CarType { get; set; }
        public string CarNumber { get; set; }
        public string CarForm { get; set; }
        public double NetPrice { get; set; }
        public int BreakPeriodForOrder { get; set; }
        public double BreakPriceForOrder { get; set; }
        public int RentalDailyPeriodForOrder { get; set; }
        public double RentailDailyPriceForOrder { get; set; }
        public double SubPrice { get; set; }
        public double DiscountValue { get; set; }
        public string? CoponCode { get; set; }
        public int BreakPeriodForRenew { get; set; }
        public double BreakPriceForRenew { get; set; }
        public int RentalDailyPeriodForRenew { get; set; }
        public double RentailDailyPriceForRenew { get; set; }
        public double RenewOrderPrice { get; set; }  // السعر الجديد 
        public double RenewRetalPeriod { get; set; }  ////مدة الاستئجار الجديد
        public string RentalCompany { get; set; }
        public List<string> DeliveyCompanies { get; set; }
        public string OrderCaseText { get; set; }
        public int OrderCase { get; set; }
        public int OrderStatus { get; set; }

    }
}
