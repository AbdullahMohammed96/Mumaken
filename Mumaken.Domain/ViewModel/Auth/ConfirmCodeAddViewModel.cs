using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Auth
{
    public class ConfirmCodeAddViewModel
    {
        public string lang { get; set; } = "ar";
        public string userId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int code { get; set; }
    }
}
