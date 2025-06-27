using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.OrderDTO
{
    public class OrderInfoDto
    {
        public int orderId { get; set; }
        public string userName { get; set; }
        public string providerName { get; set; }
        public string date { get; set; }
        public List<string> imgs { get; set; }
        public string info { get; set; }
        public int stutes { get; set; }
        public double price { get; set; }
    }
}
