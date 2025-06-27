using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.UserTables
{
    public class DataForDeliverApp
    {
        public int Id { get; set; }
        public string LoginData { get; set; }
        public string Password { get; set; }
        public int? OrderId { get; set; }
        public string DeliverCompanyId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationDbUser DeliverCompany { get; set; }
        public virtual ApplicationDbUser User { get; set; }
    }
}
