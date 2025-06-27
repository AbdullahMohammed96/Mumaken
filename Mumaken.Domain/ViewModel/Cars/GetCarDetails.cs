using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Cars
{
    public class GetCarDetails
    {
        public int Id { get; set; }
        public string CarNumber { get; set; }
        public string Image { get; set; }
        public string CarModel { get; set; }
        public int CarModelId { get; set; }
        public string CarType { get; set; }
        public int CarTypeId { get; set; }
        public string CarCategory { get; set; }
        public int CarCategoryId { get; set; }
        public double DailyPrice { get; set; }
        public string InsuranceInformation { get; set; }
        public string FileInsurancInformation { get; set; }  // ملف التامين 
        public string CarForm { get; set; }    // الاستمارة
        public string Note { get; set; }
    }
}
