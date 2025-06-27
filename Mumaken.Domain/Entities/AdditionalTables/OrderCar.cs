using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.AdditionalTables
{
    public class OrderCar
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarNumber { get; set; }
        public double RentalPriceDaily { get; set; }
        public string? InsuranceInformation { get; set; }
        public string? FileInsurancInformation { get; set; }  // ملف التامين 
        public string? CarForm { get; set; }    // الاستمارة
        public string Note { get; set; }
        public int CarCategoryId { get; set; }
        public int CarModelId { get; set; }
        public int CarTypeId { get; set; }
        public string RentalCompanyId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        [InverseProperty(nameof(Mumaken.Domain.Entities.AdditionalTables.Order.OrderCar))]
        public virtual Order Order  { get; set; }
    }
}
