using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class ForgetPasswordDto
    {
        public int code { get; set; }
        public string userId { get; set; }
    }
}
