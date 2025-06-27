using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Auth
{
    public class EditCaptainViewModel
    {
        public string Id { get; set; }
        public IFormFile?  NewImageProfile { get; set; }
        public IFormFile? NewIdentityImage { get; set; }
        public IFormFile? NewDliveryLisence { get; set; }
        public string? ImageProfile { get; set; }
        public string? IdentityImage { get; set; }
        public string? DeliveryImage { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string UserName { get; set; }
        //public string PhoneNumber { get; set; }
        //public string PhoneCode { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int GenderType { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int Nationality { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        [StringLength(10, ErrorMessage = "ValidCommercialRegisterNumber", MinimumLength = 10)]
        public string IdentityNumber { get; set; }
        //[Required(ErrorMessage = "RequiedField")]
        //public int Age { get; set; }

        [Required(ErrorMessage = "RequiedField")]
        public DateTime BirthDate { get; set; }
        public string lang { get; set; }
    }
}
