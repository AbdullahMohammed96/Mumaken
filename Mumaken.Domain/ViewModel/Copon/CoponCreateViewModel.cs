using System;
using System.ComponentModel.DataAnnotations;

namespace Mumaken.Domain.ViewModel.Copon
{
    public class CoponCreateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك هذا الحقل مطلوب")]
        [RegularExpression("([0-9]+)", ErrorMessage = "برجاء ادخال ارقام موجبة فقط")]
        public int? Count { get; set; }// عدد المستفيدين 
        //[Required(ErrorMessage = "من فضلك هذا الحقل مطلوب")]
        //[RegularExpression("([0-9]+)", ErrorMessage = "برجاء ادخال ارقام موجبة فقط")]
        //public int CountUsed { get; set; }// عدد استخدام الكوبون 
        [Required(ErrorMessage = "من فضلك هذا الحقل مطلوب")]
        public DateTime? expirdate { get; set; }// تاريخ انتهاء الخصم
        [Required(ErrorMessage = "من فضلك هذا الحقل مطلوب")]
        public string CoponCode { get; set; } // 
        [Range(0, 100, ErrorMessage = "النسبة المئوية لاتزيد عن 100")]
        [Required(ErrorMessage = "من فضلك هذا الحقل مطلوب")]
        public double? Discount { get; set; }// نسبه ياعنى 20% 
        [Required(ErrorMessage = "من فضلك هذا الحقل مطلوب")]
        [Range(0, double.MaxValue, ErrorMessage = "برجاء ادخال ارقام موجبة فقط")]
        public double? limt_discount { get; set; } //حد اقصى للخصم مثلا 50 ريال
        public bool IsActive { get; set; }// يعامل معامله الحذف 
        public string RentalCompaniesIds { get; set; } 

        public List<string> RentalCompaniesIdsList { get; set; }= new List<string>();
    }
}
