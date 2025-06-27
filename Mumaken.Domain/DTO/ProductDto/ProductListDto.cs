using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.ProductDto
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CatName { get; set; }
        public string RegionName { get; set; }
        public string Datetime { get; set; }
        public string Img { get; set; }
        public bool IsFavorite { get; set; }
    }
}
