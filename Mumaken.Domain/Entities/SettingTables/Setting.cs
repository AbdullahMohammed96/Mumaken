using System.ComponentModel.DataAnnotations;

namespace Mumaken.Domain.Entities.SettingTables
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public string InformationRenewalOrcCancellationAr { get; set; }
        public string InformationRenewalOrcCancellationEn { get; set; }
        public string IntroAppAr { get; set; }
        public string IntroAppEn { get; set; }
        public string AboutAppAr { get; set; }
        public string AboutAppEn { get; set; }
        public string AboutAppProviderAr { get; set; }
        public string AboutAppProviderEn { get; set; }
        public string CondtionsArClient { get; set; }
        public string CondtionsEnClient { get; set; }

        public string CondtionsArProvider { get; set; }
        public string CondtionsEnProvider { get; set; }

        public string AboutUsArClient { get; set; }
        public string AboutUsEnClient { get; set; }

        public string AboutUsArProvider { get; set; }
        public string AboutUsEnProvider { get; set; }
        public string PrivacyPolicyAr { get; set; }
        public string PrivacyPolicyEn { get; set; }
        public string SenderName { get; set; } = "test";
        public string UserNameSms { get; set; } = "test";
        public string PasswordSms { get; set; } = "test";
        public double  AppPercentage { get; set; }
        public  double VatPercetage { get; set; }
        public string ApplicationId { get; set; }
        public string SenderId { get; set; }

        public int PermissiblePeriod { get; set; } //الفتره المسموحه  لشركه التاجير

        //4jawaly mobily elyamam
        public string SmsProvider { get; set; } = "test";
        public string ChangeLangInformationRenewalOrcCancellation(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, InformationRenewalOrcCancellationAr, InformationRenewalOrcCancellationEn);
        }
       
        public string ChangeLangIntroApp(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, IntroAppAr, IntroAppEn);
        }
        public string ChangeLangCondtionClient(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, CondtionsArClient, CondtionsEnClient);
        }
        public string ChangeLangAboutApp(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang,AboutAppAr, AboutAppEn);
        }
        public string ChangeLangAboutAppProvider(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, AboutAppProviderAr, AboutAppProviderEn);
        }
        public string ChangeLangPrivacyPolicy(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, PrivacyPolicyAr, PrivacyPolicyEn);
        }
        public string ChangeLangCondtionProvider(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, CondtionsArProvider, CondtionsEnProvider);
        }
        public string ChangeLangAboutUsClient(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, AboutUsArClient, AboutUsEnClient);
        }
        public string ChangeLangAboutUsProvider(string lang = "ar")
        {

            return AAITHelper.HelperMsg.creatMessage(lang, AboutUsArProvider, AboutUsEnProvider);
        }
    }
}
