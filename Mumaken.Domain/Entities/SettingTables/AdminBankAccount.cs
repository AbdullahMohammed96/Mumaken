using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.SettingTables
{
    public class AdminBankAccount
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankAccountName { get; set; }
        public string Iban { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
