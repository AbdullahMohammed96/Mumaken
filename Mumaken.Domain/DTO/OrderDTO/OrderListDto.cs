using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.OrderDTO
{
    public class OrderListDto
    {
        public int orderId { get; set; }
        public string orderCaseText { get; set; }
        public int orderCase { get; set; }
        public string date { get; set; }
        public double price { get; set; }
    }
}
