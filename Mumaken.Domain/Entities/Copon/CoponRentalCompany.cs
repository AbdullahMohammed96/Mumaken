using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mumaken.Domain.Entities.UserTables;

namespace Mumaken.Domain.Entities.Copon
{
    public class CoponRentalCompany
    {
        public int Id { get; set; }
        public int CoponId { get; set; }
        public string RentalCompanyId { get; set; }

        public virtual Copon Copon { get; set; }
        public virtual ApplicationDbUser RentalCompany { get; set; }
    }
}
