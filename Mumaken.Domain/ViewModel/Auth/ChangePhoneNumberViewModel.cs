using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Auth
{
    public  class ChangePhoneNumberViewModel
    {
        public string UserId { get; set; }

        public string CurrentPasword { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string lang { get; set; }
    }
}
