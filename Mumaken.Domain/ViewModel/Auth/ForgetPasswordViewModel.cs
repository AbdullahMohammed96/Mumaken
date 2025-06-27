using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Auth
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "RequiedField")]
        //[RegularExpression("(05)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "CorrectPhoneNumber")]
        public string phone { get; set; }
        public string phoneCode { get; set; }
        public string lang { get; set; } = "ar";
    }
}
