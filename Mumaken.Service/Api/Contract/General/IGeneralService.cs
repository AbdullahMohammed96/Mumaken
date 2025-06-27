using Mumaken.Domain.Common.Helpers.ResponseHelper;
using Mumaken.Domain.DTO;
using Mumaken.Domain.DTO.GeneralDTO;
using Mumaken.Domain.Entities.AdditionalTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Api.Contract.General
{
    public interface IGeneralService
    {
        Task<BaseResponseDto<HomePageDto>> GetHomePage(string lang = "ar");
        Task<BaseResponseDto<List<OrderCycleSplashDto>>> GetOrderCycleSplash(string lang = "ar");
        Task<BaseResponseDto<List<GeneralNameDto>>> GetNationalities(string lang = "ar");
        Task<BaseResponseDto<GetAllCarResponseWithSomeSettingInformatio>> GetCars (int pageNumber, int pageSize, string lang);
        Task<BaseResponseDto<List<GeneralNameDto>>> GetAllCarType(string lang = "ar");
        Task<BaseResponseDto<List<GeneralNameDto>>> GetAllCarCatrgory(string lang = "ar");
        Task<BaseResponseDto<List<GeneralNameDto>>> GetAllCarModelWithCarType(GetAllCarModelWithTypeDto model);
        Task<BaseResponseDto<List<GeneralNameDto>>> GetAllCities(string lang = "ar");
        Task<BaseResponseDto<Pagination<GetDeliveryCompanyDto>>> GetDeliveryCompany(int pageNumber, int pageSize,int? orderId,  string lang = "ar");
        Task<BaseResponseDto<Pagination<GetCarDto>>> FilterCar(FilterRequestDto model);
    }
}
