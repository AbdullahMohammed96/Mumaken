using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.AdminOrders
{
    public class GetDeliveryCompanyOrdersByAdminViewModel
    {
        public int OrderDeliveryId { get; set; }
        public int OrderId { get; set; }
        public string Date { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string DriverLiscenceImage { get; set; }
        public string carmodel { get; set; }
        public string cartype { get; set; }
        public string carcategory { get; set; }
        public string CarNumber { get; set; }
        public int RentalPeriod { get; set; }
        public string DeliveryCompanyName { get; set; }
        public string CarForm { get; set; }

    }
}
