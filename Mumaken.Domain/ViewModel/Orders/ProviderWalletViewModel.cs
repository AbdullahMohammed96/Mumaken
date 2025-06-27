using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Orders
{
    public class ProviderWalletViewModel
    {
        public ProviderFinancialForCashOrderViewModel ProviderCashFinancial { get; set; }
        public ProviderFinancialForOnlineOrderViewModel ProviderOnlineFinancial { get; set; }
        public List<AdminBankViewModel> AdminBanks { get; set; }
    }
    public class ProviderFinancialForCashOrderViewModel
    {
        public double TotalPrice { get; set; }
        public double AppPrice { get; set; }
    }
    public class ProviderFinancialForOnlineOrderViewModel
    {
        public double TotalPrice { get; set; }
        public double ProviderPrice { get; set; }
        public double AppPrice { get; set; }
    }
    public class AdminBankViewModel
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountName { get; set; }
        public string Iban { get; set; }
        public string AccountNumber { get; set; }
    }
}
