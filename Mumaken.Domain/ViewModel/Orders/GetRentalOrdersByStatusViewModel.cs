using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Orders
{
    public class GetRentalOrdersByStatusViewModel
    {
        public int OrderId { get; set; }
        public string DeliverName { get; set; }
        public string DeliverPhone { get; set; }
        public string CreationDate { get; set; }
        public double Price { get; set; }
    }
}
