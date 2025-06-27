using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Orders
{
    public class RequestToConfirmMysettleViewModel
    {
        public string BankName { get; set; }
        public string AccountOwnerName { get; set; }
        public string AccountNumber { get; set; }
        public string Iban { get; set; }
        public string CompanyId { get; set; }
        public string lang { get; set; }
    }
}
