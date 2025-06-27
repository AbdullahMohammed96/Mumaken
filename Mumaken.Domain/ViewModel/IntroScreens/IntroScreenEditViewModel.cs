using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.IntroScreens
{
    public class IntroScreenEditViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public string TitleAr { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public string DetailsAr { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public string TitleEn { get; set; }


        [Required(ErrorMessage = "هذا الحقل مطلوب ")]
        public string DetailsEn { get; set; }

        public string? BackgroundAr { get; set; }
        public IFormFile? NewBackgroundAr { get; set; }
        public string? BackgroundEn { get; set; }
        public IFormFile? NewBackgroundEn { get; set; }
    }
}
