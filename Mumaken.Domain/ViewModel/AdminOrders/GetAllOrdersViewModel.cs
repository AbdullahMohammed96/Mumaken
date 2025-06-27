using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.AdminOrders
{
    public  class GetAllOrdersViewModel
    {
        public int OrderId { get; set; }
        public string Date { get; set; }
        public double TotalPrice { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public List<string> DliveryCompanys { get; set; }
        public string RentalCompany { get; set; }
        public int RentalPeriod { get; set; }
        public string OrderStatusText { get; set; }
        public string OrderCaseText { get; set; }
    }
}
