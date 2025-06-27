using Mumaken.Domain.DTO.SpecificationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.ProductDto
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegionName { get; set; }
        public string Date { get; set; }
        public string CatName { get; set; }
        public string ShareLink { get; set; }
        public bool IsFavorite { get; set; }
        public List<SpecificationListDto> SpecificationListDtos { get; set; }
        public List<ProductFileListDto> ProductFileListDtos { get; set; }
        public List<ProductImgListDto> ProductImgListDtos { get; set; }
        public List<ProductRelatedListDto> ProductRelatedListDtos { get; set; }
    }
}
