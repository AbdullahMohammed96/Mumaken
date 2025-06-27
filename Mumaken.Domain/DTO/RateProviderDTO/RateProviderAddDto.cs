using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.RateProviderDTO
{
    public class RateProviderAddDto
    {
        public string userId { get; set; }
        public string providerId { get; set; }
        public string userName { get; set; }
        public int rate { get; set; }
        public string lang { get; set; } = "ar";
    }
}
