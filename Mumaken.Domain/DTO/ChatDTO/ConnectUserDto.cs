using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.ChatDTO
{
    public class ConnectUserDto
    {
        public string ContextId { get; set; }
        public string FK_User { get; set; }
    }
}
