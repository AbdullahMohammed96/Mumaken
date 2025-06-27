using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Orders
{
    public class DeliveryCompanyReportViewModel
    {
        public int OrderId { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string CreationDate { get; set; }
        public string TypeDriverOrder { get; set; }
    }
}
