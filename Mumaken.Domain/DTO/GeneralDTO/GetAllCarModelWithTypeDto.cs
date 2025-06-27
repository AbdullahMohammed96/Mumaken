using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.GeneralDTO
{
    public class GetAllCarModelWithTypeDto
    {
        public List<int> typeIds { get; set; }
        public string lang { get; set; }
    }
}
