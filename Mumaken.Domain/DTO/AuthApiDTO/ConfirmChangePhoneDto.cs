using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class ConfirmChangePhoneDto
    {
        public string userId { get; set; }
        public string phoneNumber { get; set; }
        public string phoneCode { get; set; }
        public int code { get; set; }
        public string lang { get; set; }
    }
}
