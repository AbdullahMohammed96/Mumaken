using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Orders
{
    public class RequestToConfirmAdminSettleViewModel
    {
        public string CompanyId { get; set; }
        public int AdminBankId { get; set; }
        public string lang { get; set; }
    }
}
