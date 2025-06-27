using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Settings;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.SettingServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NToastNotify;
using Mumaken.Domain.Common.Helpers;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Setting)]
    public class SettingsController : Controller
    {
        private readonly ISettingServices _settingServices;
        private readonly IToastNotification _toastNotification;
        public SettingsController(ISettingServices settingServices, IToastNotification toastNotification)
        {
            _settingServices = settingServices;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var editsetting = await _settingServices.GetSetting(id);

            if (editsetting == null)
            {
                return NotFound();
            }
            return View(editsetting);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingEditViewModel editSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                var lang = Helper.GetLanguage();
                try
                {
                    await _settingServices.EditSetting(editSettingViewModel);
                }
                catch (Exception ex)
                {
                    if (! _settingServices.SettingExists(editSettingViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _toastNotification.AddSuccessToastMessage(lang == "ar" ? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
                return RedirectToAction("Index", "Home");
            }
            return View(editSettingViewModel);
        }

    }
}