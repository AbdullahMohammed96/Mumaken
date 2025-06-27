using System.ComponentModel.DataAnnotations;

namespace Mumaken.Domain.ViewModel.Payment
{
    public class PaymentViewModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string ViMaEntityId { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string MadaEntityId { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string LiveAccessToken { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string TestAccessToken { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string PaymentType { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string Currency { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public bool IsLive { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public bool IsMada { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string LiveCheckoutUrl { get; set; }
        [Required(ErrorMessage = "هذا الحقل مطلوب")]
        public string TestCheckoutUrl { get; set; }
    }
}
