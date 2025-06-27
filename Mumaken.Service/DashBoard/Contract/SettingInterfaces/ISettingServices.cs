using Mumaken.Domain.DTO.OrderDTO;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.ViewModel.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Contract.SettingServices
{
    public interface ISettingServices
    {
        Task<SettingEditViewModel> GetSetting(int? id);
        Task<bool> EditSetting(SettingEditViewModel settingEditViewModel);
        bool SettingExists(int id);
    }
}
