using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Slider
{
    public class EditSliderViewModel
    {
        public int Id { get; set; }
        public string ImgAr { get; set; }
        public IFormFile? NewImgAr { get; set; }
        public string ImgEn { get; set; }
        public IFormFile? NewImgEn { get; set; }
    }
}
