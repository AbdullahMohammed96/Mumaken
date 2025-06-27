using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.SettingTables
{
    public class FinancialAccount
    {
        public int Id { get; set; }
        public double AppTax { get; set; }
        public double TotalOrderPrice { get; set; }
        public double ProviderPrice { get; set; }
        public bool Confirm { get; set; }
        public int OrderId { get; set; }
        public int TypeOrderOrderPay { get; set; }
        public string RentalCompanyId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public virtual Order  Order { get; set; }
        [ForeignKey(nameof(RentalCompanyId))]
        public virtual ApplicationDbUser RentalCompany { get; set; }
    }
}
