using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Slider
{
    public class AddSliderViewModel
    {
        [Required(ErrorMessage = "RequiredPhoto")]
        public IFormFile ImgAr { get; set; }
        [Required(ErrorMessage = "RequiredPhoto")]
        public IFormFile ImgEn { get; set; }
    }
}
