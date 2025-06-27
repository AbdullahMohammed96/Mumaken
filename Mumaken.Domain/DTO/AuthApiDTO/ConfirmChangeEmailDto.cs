using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class ConfirmChangeEmailDto
    {
        public string email { get; set; }
        public int code { get; set; }
        public string lang { get; set; }
    }
}
