using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Orders
{
    public class EnterDataForDeliveryAppViewModel
    {
        public string LoginData { get; set; }
        public string Password { get; set; }
        public int OrderId { get; set; }
        public string  UserId{ get; set; }
        public string DeliveryCompantId { get; set; }
        public string lang { get; set; }
    }
}
