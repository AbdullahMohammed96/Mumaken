using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.IntroScreens
{
    public class IntroScreenCreateViewModel
    {
        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public string TitleAr { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public string DetailsAr { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public string TitleEn { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public string DetailsEn { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public IFormFile BackgroundImageAr { get; set; }

        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public IFormFile BackgroundImageEn { get; set; }
    }
}
