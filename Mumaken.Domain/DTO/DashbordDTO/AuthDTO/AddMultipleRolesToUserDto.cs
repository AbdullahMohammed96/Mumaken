using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.DashbordDTO.AuthDTO
{
    public class AddMultipleRolesToUserDto
    {
        public string userId { get; set; }
        public IEnumerable<string> rolesId { get; set; }
    }
}
