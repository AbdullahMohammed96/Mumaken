using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthDTO
{
    public class ConfirmCodeDto
    {
        public string userId { get; set; }
        public int typeUser { get; set; }
    }
}
