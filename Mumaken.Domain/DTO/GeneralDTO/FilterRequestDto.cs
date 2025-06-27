using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.GeneralDTO
{
    public class FilterRequestDto
    {
        public List<int> carCategoreId { get; set; }
        public List<int> carTypeId { get; set; }
        public List<int> carModelId { get; set; }
        public List<int> cityId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string lang { get; set; }
    }
}
