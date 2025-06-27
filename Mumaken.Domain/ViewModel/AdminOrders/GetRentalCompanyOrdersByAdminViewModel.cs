using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.AdminOrders
{
    public class GetRentalCompanyOrdersByAdminViewModel
    {
        public int OrderId { get; set; }
        public string Date { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string DriverLiscenceImage { get; set; }
        public string carmodel { get; set; }
        public string cartype { get; set; }
        public string carcategory { get; set; }
        public string CarNumber { get; set; }
        public double DailyRentalForCar { get; set; }
        public int RentalPeriod { get; set; }
        public int RemainPeriod { get; set; }
        public string RentalCompany { get; set; }
        public double NetPrice { get; set; }
        public int OrderStatus { get; set; }
        public string OrderStatusText { get; set; }
        public string OrderCaseText { get; set; }
        public int OrderCase { get; set; }
        public double RenewPrice { get; set; }
        public int RenewPeriod { get; set; }

        public string CarForm { get; set; }
    }
}
