using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Auth
{
    public class CheckCodeViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int Code { get; set; }
        public string lang { get; set; } = "ar";
    }
}
