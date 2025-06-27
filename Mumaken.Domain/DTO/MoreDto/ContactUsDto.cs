using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.MoreDto
{
    public class ContactUsDto
    {
        public string user_Name { get; set; }
        public string email { get; set; }
        public string phoneCode { get; set; }
        public string phone { get; set; }
        public string titleMessage { get; set; }
        public string message { get; set; }
        public string lang { get; set; }
    }
}
