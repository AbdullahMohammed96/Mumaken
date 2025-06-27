using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Region
{
    public class CreateRegoinsViewModel
    {
        [Required(ErrorMessage = "RequiedField")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "RequiedField")]
        public string NameEn { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "RequiedField")]
        public int Fk_City { get; set; }
    }
}
