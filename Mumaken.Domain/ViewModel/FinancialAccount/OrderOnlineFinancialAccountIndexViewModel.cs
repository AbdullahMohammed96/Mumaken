using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.FinancialAccount
{
    public class OrderOnlineFinancialAccountIndexViewModel
    {
        public int Id { get; set; }
        public string ProviderName { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderPhone { get; set; }
        public string ProviderBankName { get; set; }
        public string ProviderBankIBN { get; set; }
        public double TotalOrderPrice { get; set; }
        public double AppTax { get; set; }
        public double ProviderPrice { get; set; }
    }
}
