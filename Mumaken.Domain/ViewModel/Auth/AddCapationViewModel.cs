using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Auth
{
    public class AddCapationViewModel
    {
        [Required(ErrorMessage = "RequiedField")]
        public IFormFile ImageProfile { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        [RegularExpression("(5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "CorrectPhoneNumber")]
        public string Phone { get; set; }
        public string PhoneCode { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int CityId { get; set; }
        public int AddUserType { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public DateTime Birthdate { get; set; }
        //[Required(ErrorMessage = "RequiedField")]
        //public int Age { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int NationlityId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int GenderType { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        [StringLength(10, ErrorMessage = "ValidCommercialRegisterNumber", MinimumLength = 10)]
        public string IdentityNumber { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public IFormFile IdentityNumberImage { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public IFormFile DeliveryLicenseImage { get; set; }
        public string DeliverCompanyId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string Password { get; set; }
        public string lang { get; set; }

    }
}
