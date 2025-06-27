using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class ChangeEmailDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public string lang { get; set; }
    }
}
