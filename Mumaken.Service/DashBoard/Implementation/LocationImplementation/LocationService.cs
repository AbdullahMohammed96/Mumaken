using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Entities.Location;
using Mumaken.Domain.ViewModel.Region;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.LocationInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Implementation.LocationImplementation
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCity(CreateCityViewModel model)
        {
            var city = new City
            {
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                Date = DateTime.UtcNow.AddHours(3),
                IsActive = true
            };
            await _context.City.AddAsync(city);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeState(int id)
        {
            var city = await _context.City.FirstOrDefaultAsync(c => c.Id == id);
            city.IsActive = !city.IsActive;
            await _context.SaveChangesAsync();
            return city.IsActive;
        }

        public async Task<List<CitiesIndexViewModel>> GetAllCities()
        {
            var cities = await _context.City
                .OrderByDescending(c => c.Id)
                .Where(c => !c.IsDeleted)
                .Select(c => new CitiesIndexViewModel
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    IsActive = c.IsActive
                }).ToListAsync();
            return cities;
        }

        public Task<List<SelectListItem>> GetCitiesSelectList(int? cityId,string lang="ar")
        {
            var cities = _context.City.Where(c => c.IsActive)
                   .Select(c => new SelectListItem
                   {
                       Value = c.Id.ToString(),
                       Text = c.ChangeLangName(lang),
                       Selected = cityId != null ? c.Id == cityId.Value : false
                   }).ToListAsync();
            return cities;
        }

        public async Task<EditCityViewModel> GetCity(int id)
        {
            var city = await _context.City
                .Where(c => c.Id == id)
                .Select(c => new EditCityViewModel
                {
                    Id = c.Id,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,

                }).FirstOrDefaultAsync();
            return city;
        }

        public async Task<bool> UpdateCity(EditCityViewModel model)
        {
            var city = await _context.City.FirstOrDefaultAsync(c => c.Id == model.Id);
            city.NameEn = model.NameEn;
            city.NameAr = model.NameAr;
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<List<GetAllRegionViewModel>> GetAllRegion()
        {
            var regions = await _context.Distract
                   .OrderByDescending(c=>c.Id)
                  .Include(c => c.City)
                  .Select(c => new GetAllRegionViewModel
                  {
                      Id = c.Id,
                      NameAr = c.NameAr,
                      NameEn = c.NameEn,
                      IsActive = c.IsActive,
                      CityName = c.City.NameAr,
                  }).ToListAsync();
            return regions;
        }

        public async Task<List<GetAllRegionViewModel>> GetAllRegionIncity(int cityId)
        {
            var regions = await _context.Distract
                 .Where(c => c.CityId == cityId)
                  .Include(c => c.City)
                  .Select(c => new GetAllRegionViewModel
                  {
                      Id = c.Id,
                      NameAr = c.NameAr,
                      NameEn = c.NameEn,
                      IsActive = c.IsActive,
                      CityName = c.City.NameAr,
                  }).ToListAsync();
            return regions;
        }
        public async Task<bool> ChangeRegionState(int id)
        {
            var district = await _context.Distract.FirstOrDefaultAsync(c => c.Id == id);
            district.IsActive = !district.IsActive;
            _context.Distract.Update(district);
            await _context.SaveChangesAsync();
            return district.IsActive;
        }
        public async Task<bool> AddRegion(CreateRegoinsViewModel model)
        {
            var region = new Distract
            {
                NameAr = model.NameAr,
                NameEn = model.NameEn,
                CityId = model.Fk_City,
                IsActive = true,
                Date = DateTime.UtcNow.AddHours(3),
            };
            var city = await _context.City.FirstOrDefaultAsync(c => c.Id == model.Fk_City);
            if (city.IsActive)
            {
                try
                {
                    _context.Distract.Add(region);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            throw new Exception("لا يمكنك إضافه حي لمدينه غير مفعاله");

        }
        public async Task<EditRegoinViewModel> GetRegion(int id)
        {
            var region = await _context.Distract.Where(c => c.Id == id).Select(c => new EditRegoinViewModel
            {
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                Fk_City = c.CityId

            }).FirstOrDefaultAsync();
            return region;
        }
        public async Task<bool> UpdateRegion(EditRegoinViewModel model)
        {
            var region = await _context.Distract.FirstOrDefaultAsync(c => c.Id == model.Id);
            region.NameAr = model.NameAr;
            region.NameEn = model.NameEn;
            region.CityId = model.Fk_City;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
