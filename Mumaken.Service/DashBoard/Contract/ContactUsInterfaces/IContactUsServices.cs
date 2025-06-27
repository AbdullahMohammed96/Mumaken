using Table= Mumaken.Domain.Entities.SettingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Contract.ContactUsInterfaces
{
    public interface IContactUsServices
    {
        Task<List<Table.ContactUs>> GetContactUs();
        Task<bool> DeleteContactUs(int? id);
    }
}
