using Mumaken.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.Entities.CarTable
{
    public class Car
    {
        public int Id { get; set; }
        public string CarImage { get; set; }
        public string CarNumber{ get; set; }
        public double RentalPriceDaily { get; set; }
        //public string InsuranceInformation { get; set; }
        //public string FileInsurancInformation { get; set; }  // ملف التامين 
        //public string CarForm { get; set; }    // الاستمارة
        public string Note { get; set; }
        public int CarCategoryId { get; set; }
        public int CarModelId { get; set; }
        public int CarTypeId { get; set; }
        public string RentalCompanyId { get; set; }
        public bool IsDeleted { get; set; }
        // Relation
        [ForeignKey(nameof(CarModelId))]
        public virtual CarModel CarModel { get; set; }  //  موديل السيارة
        [ForeignKey(nameof(CarTypeId))]
        public virtual CarType CarType   { get; set; }   // نوع السياره او الماركه
        [ForeignKey(nameof(CarCategoryId))]
        public virtual CarCategory CarCategory { get; set; }   // فئه السياره
        [ForeignKey(nameof(RentalCompanyId))]
        [InverseProperty(nameof(ApplicationDbUser.Cars))]
        public ApplicationDbUser RentalCompany{ get; set; }
    }
}
