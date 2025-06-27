using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.ProductDto
{
    public class ProductFilterDto
    {
        public string SearchString { get; set; } = "";
        public string Lang { get; set; } = "ar";
        public int PageNumber { get; set; } = 1;
        public int CityId { get; set; } = 0;
        public int RegionId { get; set; } = 0;
        public int CategoryId { get; set; } = 0;
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        public DateTime DateTo { get; set; } = DateTime.MinValue;
    }
}
