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
    public class Report
    {
        public int Id { get; set; }
        public string RentalCompayId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(RentalCompayId))]
        public virtual ApplicationDbUser RentalCompany { get; set; }

    }
}
