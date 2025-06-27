using Microsoft.AspNetCore.Http;
using Mumaken.Domain.Entities.CarTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Cars
{
    public class AddCarViewModel
    {
        [Required(ErrorMessage = "RequiedField")]
        public IFormFile CarImage { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string CarNumber { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        [Range(1,double.MaxValue,ErrorMessage = "ValidPrice")]
        public double RentalPriceDaily { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string Note { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int CarCategoryId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int CarModelId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int CarTypeId { get; set; }
        public string RentalCompanyId { get; set; }
        //[Required(ErrorMessage = "RequiedField")]
        //public string InsuranceInformation { get; set; }
        //[Required(ErrorMessage = "RequiedField")]
        //public IFormFile FileInsurancInformation { get; set; }  // ملف التامين 
        //[Required(ErrorMessage = "RequiedField")]
        //public IFormFile CarForm { get; set; }    // الاستمارة
        public string? lang { get; set; }

        public static Car MappedToCar(AddCarViewModel model)
        {
            var car = new Car
            {
                CarNumber=model.CarNumber,
                CarCategoryId=model.CarCategoryId,
                CarTypeId=model.CarTypeId,
                CarModelId=model.CarModelId,
                RentalPriceDaily=model.RentalPriceDaily,
                //InsuranceInformation=model.InsuranceInformation,
                Note=model.Note,
                RentalCompanyId=model.RentalCompanyId,
            };
            return car;
        }

    }
}
