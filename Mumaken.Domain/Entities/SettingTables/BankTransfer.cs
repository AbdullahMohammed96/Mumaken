using Mumaken.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.SettingTables
{
    public class BankTransfer
    {
        public int Id { get; set; }
        public double Total { get; set; }
        public double AppTax { get; set; }
        public bool Confirm { get; set; }
        public int TypeOrderOrderPay { get; set; }  // cash Or  online
        public string? BankName { get; set; }
        public string? Iban { get; set; }
        public string? AccountOwnerName { get; set; }
        public string? AccountNumber { get; set; }
        public int? AdminBankId { get; set; }
        [ForeignKey(nameof(AdminBankId))]
        public virtual AdminBankAccount AdminBank { get; set; }
        public string ProviderId { get; set; }
        [ForeignKey(nameof(ProviderId))]
        public virtual  ApplicationDbUser Provider { get; set; }
    }
}
