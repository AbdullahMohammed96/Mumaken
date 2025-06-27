using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.OrderDTO
{
    public class GetOrderByStatusDto
    {
        public int status { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string lang { get; set; }
    }
}
