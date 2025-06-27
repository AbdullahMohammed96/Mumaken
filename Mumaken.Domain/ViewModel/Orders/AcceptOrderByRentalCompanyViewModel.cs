using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Orders
{
    public class AcceptOrderByRentalCompanyViewModel
    {
        public int OrderId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public DateTime datePickupCar { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public DateTime TimePickupCar { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string locationPickupCar { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string InsuranceInformation { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public IFormFile FileInsurancInformation { get; set; }  // ملف التامين 
        [Required(ErrorMessage = "RequiedField")]
        public IFormFile CarForm { get; set; }    // الاستمارة
        public string lat { get; set; }
        public string lng { get; set; }
        public string lang { get; set; }
    }
}
