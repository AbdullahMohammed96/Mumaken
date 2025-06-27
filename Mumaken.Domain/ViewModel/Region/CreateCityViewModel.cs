using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Region
{
    public class CreateCityViewModel
    {
        [Required(ErrorMessage = "RequiedField")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string NameEn { get; set; }
    }
}
