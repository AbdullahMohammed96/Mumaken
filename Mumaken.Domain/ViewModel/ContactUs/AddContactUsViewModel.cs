using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.ContactUs
{
    public class AddContactUsViewModel
    {
        [Required(ErrorMessage = "RequiedField")]
        public string MessageSubject { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string Message { get; set; }
    }
}
