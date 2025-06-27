using Microsoft.AspNetCore.Mvc.Rendering;
using Mumaken.Domain.ViewModel.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Contract.LocationInterfaces
{
    public interface ILocationService
    {
        Task<List<CitiesIndexViewModel>> GetAllCities();
        Task<bool> AddCity(CreateCityViewModel model);
        Task<EditCityViewModel> GetCity(int id);
        Task<bool> UpdateCity(EditCityViewModel model);
        Task<bool> ChangeState(int id);
        Task<List<SelectListItem>> GetCitiesSelectList(int? cityId, string lang = "ar");
        Task<List<GetAllRegionViewModel>> GetAllRegion();
        Task<List<GetAllRegionViewModel>> GetAllRegionIncity(int cityId);
        Task<bool> ChangeRegionState(int id);
        Task<bool> AddRegion(CreateRegoinsViewModel model);
        Task<bool> UpdateRegion(EditRegoinViewModel model);
        Task<EditRegoinViewModel> GetRegion(int id);
    }
}
