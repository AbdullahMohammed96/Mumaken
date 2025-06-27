using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class ChangePhoneDto
    {
        public string userId { get; set; }
        public string password { get; set; }
        public string phoneCode { get; set; }
        public string phone { get; set; }
        public string lang { get; set; }
    }
}
