using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthDTO
{
    public class ResendCodeDto
    {
        public int code { get; set; }
        public string userId { get; set; }
        public string phone { get; set; }
    }
}
