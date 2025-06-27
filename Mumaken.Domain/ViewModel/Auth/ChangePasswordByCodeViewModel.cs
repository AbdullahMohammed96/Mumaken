using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Auth
{
    public class ChangePasswordByCodeViewModel
    {
        public string userId { get; set; }
        [Required(ErrorMessage = "من  فضلك ادحل كلمه المرور")]
        [MinLength(6, ErrorMessage = "كلمه المرور لا يجب ان تقل عن 6 ارقام")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "من  فضلك ادحل كلمه المرور")]
        [MinLength(6, ErrorMessage = "كلمه المرور لا يجب ان تقل عن 6 ارقام")]
        [Compare("newPassword", ErrorMessage = "ConfirmPasswordAndPasswordCheck")]
        public string confirmNewPassword { get; set; }

        public string lang { get; set; } = "ar";
    }
}
