using Microsoft.AspNetCore.Mvc.Rendering;
using Mumaken.Domain.DTO;
using Mumaken.Domain.ViewModel.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Contract.CarInterfaces
{
    public  interface ICarServices
    {
        Task<List<SelectListItem>> GetAllCarCatrgorySelectdList(int? carCategorySelected, string lang = "ar");
        Task<List<SelectListItem>> GetAllCarTypeSelectdList(int? carTypeSelected, string lang = "ar");
        Task<List<GeneralCarDataViewModel>> GetAllCarModelWithTypeSelectdList(int? carModelSelected, int typeId, string lang = "ar");
        Task<BaseResponseDto<object>> AddCar(AddCarViewModel model);
        Task<BaseResponseDto<List<ListCarViewModel>>> GetAllCar(string rentalCompanyId, string lang = "ar");
        Task<BaseResponseDto<GetCarDetails>> CarDetails(int carId, string lang = "ar");
        Task<BaseResponseDto<object>> UpdateCar(UpadateCarViewModel model);
        Task<BaseResponseDto<object>> DeleteCar(int carId, string lang = "ar");


    }
}
