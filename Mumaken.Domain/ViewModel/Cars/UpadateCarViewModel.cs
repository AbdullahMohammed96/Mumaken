using Microsoft.AspNetCore.Http;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.Entities.CarTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Cars
{
    public class UpadateCarViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string CarNumber { get; set; }
        public IFormFile? NewImage { get; set; }
        public string? OldCarImage { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int CarModel { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int CarType { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int CarCategory { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        [Range(1, double.MaxValue, ErrorMessage = "ValidPrice")]
        public double DailyPrice { get; set; }
        //[Required(ErrorMessage = "RequiedField")]
        //public string InsuranceInformation { get; set; }
        //public IFormFile? NewFileInsurancInformation { get; set; }  // ملف التامين 
        //public string? OldFileInsurancInformation { get; set; }
        //public IFormFile?  NewCarForm { get; set; }    // الاستمارة
        //public string? OldCarForm { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string Note { get; set; }
        public string? lang { get; set; }
        public static Car MappedToCar(UpadateCarViewModel updatedCar, Car car)
        {
            car.CarTypeId = updatedCar.CarType;
            car.CarCategoryId = updatedCar.CarCategory;
            car.CarModelId = updatedCar.CarModel;
            car.RentalPriceDaily = updatedCar.DailyPrice;
           // car.InsuranceInformation = updatedCar.InsuranceInformation;
            car.Note = updatedCar.Note;
            car.CarNumber = updatedCar.CarNumber;   
            return car;
        }
        public static UpadateCarViewModel MappDetailsCarToUpdateModel(GetCarDetails detailsCar,string lang)
        {
            return new UpadateCarViewModel
            {
                CarNumber = detailsCar.CarNumber,
                CarCategory=detailsCar.CarCategoryId,
                CarModel=detailsCar.CarModelId,
                CarType=detailsCar.CarTypeId,
                DailyPrice=detailsCar.DailyPrice,
                Note=detailsCar.Note,
                Id=detailsCar.Id,
                //InsuranceInformation=detailsCar.InsuranceInformation,
                OldCarImage= detailsCar.Image,
                //OldCarForm= detailsCar.CarForm,
                //OldFileInsurancInformation= detailsCar.FileInsurancInformation,
                lang= lang
            };
        }
    }
}
