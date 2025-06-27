using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers.Extensions;
using Mumaken.Domain.Common.Helpers.ResponseHelper;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO;
using Mumaken.Domain.DTO.GeneralDTO;
using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Enums;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.General;
using Mumaken.Service.Api.Implementation.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Api.Implementation.General
{
    public class GeneralService : IGeneralService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAccountService _accountService;
        public GeneralService(ApplicationDbContext db, IAccountService accountService)
        {
            _db = db;
            _accountService = accountService;
        }

        public async Task<BaseResponseDto<HomePageDto>> GetHomePage(string lang = "ar")
        {
            var homepage = await (from settting in _db.Settings
                                  select new HomePageDto
                                  {
                                      introApp = settting.ChangeLangIntroApp(lang),
                                      slider = _db.Sliders
                                      .Where(c => c.IsActive)
                                      .Select(c => DashBordUrl.baseUrlHost +lang=="ar" ?c.ImageAr:c.ImageEn)
                                      .ToList()
                                  }).FirstOrDefaultAsync();
            return ResponseHelper.Success(homepage);
        }
        public async Task<BaseResponseDto<List<OrderCycleSplashDto>>> GetOrderCycleSplash(string lang = "ar")
        {
            var orderSplash = await _db.OrderCycleIntros
                 .Where(c => c.IsActive)
                 .Select(c => new OrderCycleSplashDto
                 {
                     title = c.ChangeLangTitle(lang),
                     description = c.ChangeLangDescription(lang),
                     index = c.Index
                 }).OrderBy(c => c.index)
                 .ToListAsync();
            return ResponseHelper.Success(orderSplash);
        }
        public async Task<BaseResponseDto<List<GeneralNameDto>>> GetNationalities(string lang = "ar")
        {
            var nationaliles = await _db.Nationalities
                 .Where(c => c.IsActive)
                 .Select(c => new GeneralNameDto
                 {
                     id = c.Id,
                     name = c.ChangeLangName(lang),
                 }).ToListAsync();
            return ResponseHelper.Success(nationaliles);
        }
        //public async Task<BaseResponseDto<Pagination<GetCarDto>>> GetCars(int pageNumber, int pageSize, string lang)
        public async Task<BaseResponseDto<GetAllCarResponseWithSomeSettingInformatio>> GetCars(int pageNumber, int pageSize, string lang)
        {
            //var currentUser = await _accountService.GetCurrentUser();
            var cars = await _db.Cars
                .Include(c => c.CarType)
                .Include(c => c.CarCategory)
                .Include(c => c.CarModel)
                .Include(c => c.RentalCompany)
                //.Where(c => c.RentalCompany.CityId == currentUser.CityId)
                .Where(c => c.RentalCompany.IsAppravel == true&&c.RentalCompany.IsActive)
                .Select(c => new GetCarDto
                {
                    Id = c.Id,
                    image = DashBordUrl.baseUrlHost + c.CarImage,
                    note = c.Note,
                    carCategory = c.CarCategory.ChangeLangName(lang),
                    carModel = c.CarModel.ChangeLangName(lang),
                    carType = c.CarType.ChangeLangName(lang),
                    dailyRentalPrice = c.RentalPriceDaily,
                    CarRentalInfo = new CarRentalInformationDto
                    {
                        id = c.RentalCompany.Id,
                        email = c.RentalCompany.Email,
                        name = c.RentalCompany.user_Name,
                        phone = c.RentalCompany.PhoneNumber,
                        image = DashBordUrl.baseUrlHost + c.RentalCompany.ImgProfile,
                        location = c.RentalCompany.Location,
                        lat = c.RentalCompany.Lat,
                        lng = c.RentalCompany.Lng,
                        commercialRegisterNumber=c.RentalCompany.CommercialRegisterNumber
                    }
                })
                .OrderBy(c => c.dailyRentalPrice)
                .Paginate(pageNumber, pageSize);
            var (text, vatPercentage) = await GetVatPercentageWithTextRenew(lang);

            var reponse = new GetAllCarResponseWithSomeSettingInformatio
            {
                cars = cars,
                textRenewAndCancelAr = text,
                vatPercetage = vatPercentage
            };
            // return ResponseHelper.Success(cars);
            return ResponseHelper.Success(reponse);
        }
        public async Task<BaseResponseDto<Pagination<GetCarDto>>> FilterCar(FilterRequestDto model)
        {
            var query = _db.Cars
                .Include(c => c.CarType)
                .Include(c => c.CarCategory)
                .Include(c => c.CarModel)
                .Include(c => c.RentalCompany)
                  .Where(c => c.RentalCompany.IsAppravel == true && c.RentalCompany.IsActive);
            if (model.carCategoreId is not null && model.carCategoreId.Any())
            {
                query = query.Where(c => model.carCategoreId.Contains(c.CarCategoryId));
            }
            if (model.carTypeId is not null && model.carTypeId.Any())
            {
                query = query.Where(c => model.carTypeId.Contains(c.CarTypeId));
            }
            if (model.carModelId is not null && model.carModelId.Any())
            {
                query = query.Where(c => model.carModelId.Contains(c.CarModelId));
            }
            if (model.cityId is not null && model.cityId.Any())
            {
                query = query.Where(c => model.cityId.Contains(c.RentalCompany.CityId));
            }
            var filterCars = await query.Select(c => new GetCarDto
            {
                Id = c.Id,
                image = DashBordUrl.baseUrlHost + c.CarImage,
                note = c.Note,
                carCategory = c.CarCategory.ChangeLangName(model.lang),
                carModel = c.CarModel.ChangeLangName(model.lang),
                carType = c.CarType.ChangeLangName(model.lang),
                dailyRentalPrice = c.RentalPriceDaily,
                CarRentalInfo = new CarRentalInformationDto
                {
                    id = c.RentalCompany.Id,
                    email = c.RentalCompany.Email,
                    name = c.RentalCompany.user_Name,
                    phone = c.RentalCompany.PhoneNumber,
                    image = DashBordUrl.baseUrlHost + c.RentalCompany.ImgProfile,
                    location = c.RentalCompany.Location,
                    lat = c.RentalCompany.Lat,
                    lng = c.RentalCompany.Lng,
                    commercialRegisterNumber = c.RentalCompany.CommercialRegisterNumber,
                }
            }).Paginate(model.pageNumber, model.pageSize);
            return ResponseHelper.Success(filterCars);
        }
        public async Task<BaseResponseDto<List<GeneralNameDto>>> GetAllCarCatrgory(string lang = "ar")
        {
            var carCategories = await _db.CarCategories.Where(c => c.IsActive)
                 .Select(c => new GeneralNameDto
                 {
                     id = c.Id,
                     name = c.ChangeLangName(lang)
                 }).ToListAsync();
            return ResponseHelper.Success(carCategories);
        }
        public async Task<BaseResponseDto<List<GeneralNameDto>>> GetAllCarType(string lang = "ar")
        {
            var carTypes = await _db.CarTypes.Where(c => c.IsActive)
                 .Select(c => new GeneralNameDto
                 {
                     id = c.Id,
                     name = c.ChangeLangName(lang)
                 }).ToListAsync();
            return ResponseHelper.Success(carTypes);
        }
        public async Task<BaseResponseDto<List<GeneralNameDto>>> GetAllCarModelWithCarType(GetAllCarModelWithTypeDto model)
        {

            var carModels = await _db.CarModels
                .Where(c => c.IsActive && model.typeIds.Contains(c.CarTypeId))
                .Select(c => new GeneralNameDto
                {
                    id = c.Id,
                    name = c.ChangeLangName(model.lang)
                }).ToListAsync();
            return ResponseHelper.Success(carModels);
        }
        public async Task<BaseResponseDto<List<GeneralNameDto>>> GetAllCities(string lang = "ar")
        {
            var cities = await _db.City
                  .Where(c => c.IsActive)
                  .Select(c => new GeneralNameDto
                  {
                      id = c.Id,
                      name = c.ChangeLangName(lang)
                  }).ToListAsync();
            return ResponseHelper.Success(cities);
        }
        public async Task<BaseResponseDto<Pagination<GetDeliveryCompanyDto>>> GetDeliveryCompany(int pageNumber, int pageSize, int? orderId, string lang = "ar")
        {
            var deliverCompaniesQuery = _db.Users
                    .Where(c => c.IsActive
                           && c.TypeUser == (int)UserType.DeliverCompany
                           && c.IsAppravel == true);
            if (orderId.HasValue)
            {
                deliverCompaniesQuery = deliverCompaniesQuery.Where(c => !_db.OrderDeliverCompanies
                    .Any(odc => odc.DeliverCompanyId == c.Id && odc.OrderId == orderId.Value));
            }
            var deliverCompanies = await deliverCompaniesQuery
                  .Select(c => new GetDeliveryCompanyDto
                  {
                      id = c.Id,
                      name = c.user_Name
                  })
                  .Paginate(pageNumber, pageSize);
            //var deliverCompanies = await _db.Users
            //      .Where(c => c.IsActive
            //      && c.TypeUser == (int)UserType.DeliverCompany 
            //      && c.IsAppravel == true)
            //      .Select(c => new GetDeliveryCompanyDto
            //      {
            //          id = c.Id,
            //          name = c.user_Name
            //      }).Paginate(pageNumber, pageSize);
            return ResponseHelper.Success(deliverCompanies);
        }

        public async Task<(string, double)> GetVatPercentageWithTextRenew(string lang)
        {
            var setting = await _db.Settings.FirstOrDefaultAsync();
            return (setting.ChangeLangInformationRenewalOrcCancellation(lang), setting.VatPercetage);
        }
    }
}
