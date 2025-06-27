using Mumaken.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.AdditionalTables
{
    public class OrderDeliverCompany
    {
        public int Id { get; set; }
        public int DeliverCompanyCase { get; set; }
        public string DeliverCompanyId { get; set; }
        public string? UserDataInAppDelivery  { get; set; }
        public bool WorkWithDeliveryCompany { get; set; }
        public int TypeDriverInDeliveryCompanies { get; set; }
        public bool ConfirmDataOfDeliveryApp { get; set; }  // this bool For TypeDriverInDeliveryCompanies==Update 
        public string? ReasonRefused { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Mumaken.Domain.Entities.AdditionalTables.Order.DeliverCompany))]
        public virtual Order  Order { get; set; }
        [ForeignKey(nameof(DeliverCompanyId))]
        public virtual ApplicationDbUser DeliverCompany { get; set; }

    }
}
