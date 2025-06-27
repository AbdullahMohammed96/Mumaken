using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.FinancialAccount
{
    public class OrderCashFinancialAccountIndexViewModel
    {
        public int Id { get; set; }
        public string ProviderName { get; set; }
        public string ProviderEmail { get; set; }
        public string ProviderPhone { get; set; }
        public string AdminBankBankName { get; set; }
        public string AdminBankBankIBN { get; set; }
        public double TotalOrderPrice { get; set; }
        public double AppTax { get; set; }
        public double ProviderPrice { get; set; }
    }
}
