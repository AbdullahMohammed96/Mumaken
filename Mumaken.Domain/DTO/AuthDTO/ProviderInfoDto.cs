using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthDTO
{
    public class ProviderInfoDto: BaseInfoDto
    {
        public string distractName { get; set; }
        public int distractId { get; set; }
        public string location { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string commercialRegisterNumber { get; set; }
       
    }
}
