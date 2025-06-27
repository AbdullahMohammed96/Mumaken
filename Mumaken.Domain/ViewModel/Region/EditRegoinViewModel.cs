using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Region
{
    public class EditRegoinViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم الحي بالعربية ")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم الحي بالانجليزية")]
        public string NameEn { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "من فضلك اختر مدينتك")]
        public int Fk_City { get; set; }
    }
}
