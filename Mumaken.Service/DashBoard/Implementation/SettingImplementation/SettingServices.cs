using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.ViewModel.Settings;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.SettingServices;
using Microsoft.EntityFrameworkCore;

namespace Mumaken.Service.DashBoard.Implementation.SettingServices
{
    public class SettingServices : ISettingServices
    {
        private readonly ApplicationDbContext _context;

        public SettingServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<SettingEditViewModel> GetSetting(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var setting = await _context.Settings.FindAsync(id);

            SettingEditViewModel editsetting = new SettingEditViewModel
            {
                Id = setting.Id,
                CondtionsArClient = setting.CondtionsArClient,
                CondtionsEnClient = setting.CondtionsEnClient,
                CondtionsArDelegt = setting.CondtionsArProvider,
                CondtionsEnDelegt = setting.CondtionsEnProvider,
                AboutUsArClient = setting.AboutUsArClient,
                AboutUsEnClient = setting.AboutUsEnClient,
                //AboutUsArDelegt = setting.AboutUsArProvider,
                //AboutUsEnDelegt = setting.AboutUsEnProvider,
                AboutAppAr=setting.AboutAppAr,
                AboutAppEn=setting.AboutAppEn,
                AboutAppProviderAr=setting.AboutAppProviderAr, 
                AboutAppProviderEn=setting.AboutAppProviderEn,
                PrivacyPolicyEn=setting.PrivacyPolicyEn,
                PrivacyPolicyAr=setting.PrivacyPolicyAr,
                InformationRenewalOrcCancellationAr=setting.InformationRenewalOrcCancellationAr,
                InformationRenewalOrcCancellationEn=setting.InformationRenewalOrcCancellationEn,
                IntroAppAr=setting.IntroAppAr,
                IntroAppEn=setting.IntroAppEn,
                ApplicationId = setting.ApplicationId,
                SenderName = setting.SenderName,
                PasswordSms = setting.PasswordSms,
                Phone = setting.Phone,
                SenderId = setting.SenderId,
                UserNameSms = setting.UserNameSms,
                Email = setting.Email,
                Lat = setting.Lat,
                Lng = setting.Lng,
                Address = setting.Address,
                AppPercentage = setting.AppPercentage,
                VatPercetage=setting.VatPercetage,
                PermissiblePeriod=setting.PermissiblePeriod,
            };
            if (setting == null)
            {
                return null;
            }

            return editsetting;
        }

        public async Task<bool> EditSetting(SettingEditViewModel editSettingViewModel)
        {
            Setting setting = await _context.Settings.FindAsync(editSettingViewModel.Id);
            setting.Id = editSettingViewModel.Id;
            setting.CondtionsArClient = editSettingViewModel.CondtionsArClient;
            setting.CondtionsEnClient = editSettingViewModel.CondtionsEnClient;
            setting.CondtionsArProvider = editSettingViewModel.CondtionsArDelegt;
            setting.CondtionsEnProvider = editSettingViewModel.CondtionsEnDelegt;
            setting.AboutUsArClient = editSettingViewModel.AboutUsArClient;
            setting.AboutUsEnClient = editSettingViewModel.AboutUsEnClient;
            //setting.AboutUsArProvider = editSettingViewModel.AboutUsArDelegt;
            //setting.AboutUsEnProvider = editSettingViewModel.AboutUsEnDelegt;
            setting.AboutAppAr=editSettingViewModel.AboutAppAr;
            setting.AboutAppEn=editSettingViewModel.AboutAppEn;
            setting.PrivacyPolicyAr = editSettingViewModel.PrivacyPolicyAr;
            setting.PrivacyPolicyEn=editSettingViewModel.PrivacyPolicyEn;
            setting.InformationRenewalOrcCancellationAr = editSettingViewModel.InformationRenewalOrcCancellationAr;
            setting.InformationRenewalOrcCancellationEn = editSettingViewModel.InformationRenewalOrcCancellationEn;
            setting.IntroAppAr = editSettingViewModel.IntroAppAr;
            setting.IntroAppEn = editSettingViewModel.IntroAppEn;
            setting.ApplicationId = editSettingViewModel.ApplicationId;
            setting.SenderName = editSettingViewModel.SenderName;
            setting.PasswordSms = editSettingViewModel.PasswordSms;
            setting.Phone = editSettingViewModel.Phone;
            setting.SenderId = editSettingViewModel.SenderId;
            setting.UserNameSms = editSettingViewModel.UserNameSms;
            setting.Email = editSettingViewModel.Email;
            setting.Lat = editSettingViewModel.Lat;
            setting.Lng = editSettingViewModel.Lng;
            setting.Address = editSettingViewModel.Address;
            setting.AppPercentage = editSettingViewModel.AppPercentage;
            setting.VatPercetage=editSettingViewModel.VatPercetage;
            setting.AboutAppProviderEn = editSettingViewModel.AboutAppProviderEn;
            setting.AboutAppProviderAr=editSettingViewModel.AboutAppProviderAr;
            setting.PermissiblePeriod = editSettingViewModel.PermissiblePeriod;
            return await _context.SaveChangesAsync() > 0;
        }

        public bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}
