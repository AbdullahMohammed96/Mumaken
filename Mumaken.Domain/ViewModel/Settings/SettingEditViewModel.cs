using System.ComponentModel.DataAnnotations;

namespace Mumaken.Domain.ViewModel.Settings
{
    public class SettingEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        [MinLength(10, ErrorMessage = "PhoneLength")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string CondtionsArClient { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string CondtionsEnClient { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string CondtionsArDelegt { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string CondtionsEnDelegt { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string AboutUsArClient { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string AboutUsEnClient { get; set; }
        //[Required(ErrorMessage = "RequiedField")]
        //public string AboutUsArDelegt { get; set; }
        //[Required(ErrorMessage = "RequiedField")]
        //public string AboutUsEnDelegt { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string AboutAppAr { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string AboutAppEn { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string AboutAppProviderAr { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string AboutAppProviderEn { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string PrivacyPolicyAr { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string PrivacyPolicyEn { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string InformationRenewalOrcCancellationAr { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string InformationRenewalOrcCancellationEn { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string IntroAppAr { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string IntroAppEn { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string SenderName { get; set; } = "test";
        [Required(ErrorMessage = "RequiedField")]
        public double AppPercentage { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public int PermissiblePeriod { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public double VatPercetage { get; set; }
        public string UserNameSms { get; set; } = "test";
        public string PasswordSms { get; set; } = "test";
        public string ApplicationId { get; set; }
        public string SenderId { get; set; }
        [Required(ErrorMessage = "RequiedField")]
        public string Email { get; set; }

        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }

    }
}
