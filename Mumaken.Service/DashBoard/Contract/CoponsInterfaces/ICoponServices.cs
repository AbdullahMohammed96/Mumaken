using Microsoft.AspNetCore.Mvc.Rendering;
using Mumaken.Domain.Entities.Copon;
using Mumaken.Domain.ViewModel.Copon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Contract.CoponsInterfaces
{
    public interface ICoponServices
    {
        Task<List<CoponViewModel>> GetCopons();
        Task<bool> CreateCopon(CoponCreateViewModel createCoponViewModel);
        Task<Copon> GetCopon(int? CoponId);
        Task<bool> EditCopon(int id, CoponCreateViewModel createCoponViewModel);
        bool IsExist(string CoponCode);
        bool IsExist(int? CoponId);
        Task<bool> ChangeState(int? id);
        Task<bool> DeleteCopons(int? id);
        Task<List<SelectListItem>> GetRentalCompany(string id = null, string lang = "ar");

    }
}
