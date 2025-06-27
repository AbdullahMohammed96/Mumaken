using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.RateProviderDTO
{
    public class RateProviderListDto
    {
        public int id { get; set; }
        public string imgProfile { get; set; }
        public string userName { get; set; }
        public int rate { get; set; }
    }
}
