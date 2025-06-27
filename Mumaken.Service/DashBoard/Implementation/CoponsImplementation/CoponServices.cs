using Mumaken.Domain.Entities.Copon;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.ViewModel.Copon;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.CoponsInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mumaken.Domain.Enums;

namespace Mumaken.Service.DashBoard.Implementation.CoponsImplementation
{
    public class CoponServices : ICoponServices
    {
        private readonly ApplicationDbContext _context;

        public CoponServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CoponViewModel>> GetCopons()
        {
            var copons = await _context.Copon
                .Include(c => c.RentalCompany)
                .OrderByDescending(x => x.Date)
                .Select(cop => new CoponViewModel
                {
                    Id = cop.Id,
                    count = cop.Count,
                    coponCode = cop.CoponCode,
                    countUsed = cop.CountUsed,
                    discount = cop.Discount,
                    limtdiscount = cop.limtDiscount,
                    expirdate = cop.Expirdate.ToString("dd/MM/yyyy"),
                    isActive = cop.IsActive,
                    //rentalCompanyName=cop.RentalCompany!=null?cop.RentalCompany.user_Name:""
                }).ToListAsync();

            return copons;
        }

        public async Task<bool> CreateCopon(CoponCreateViewModel coponVm)
        {
            var rentalCompanies = coponVm.RentalCompaniesIds.Split(',').ToList();

            var copon = new Copon()
            {
                IsActive = true,
                Date = DateTime.Now,
                Count = (int)coponVm.Count,
                Expirdate = (DateTime)coponVm.expirdate,
                CoponCode = coponVm.CoponCode,
                //CountUsed = createCoponViewModel.CountUsed,
                Discount = (double)coponVm.Discount,
                limtDiscount = (double)coponVm.limt_discount,
                CoponRentalCompanies = rentalCompanies
                .Select(x => new CoponRentalCompany
                {
                    RentalCompanyId = x.ToString(),
                }).ToList()
                //RentalCompanyId= createCoponViewModel.RentalCompanyId
            };


            await _context.Copon.AddAsync(copon);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Copon> GetCopon(int? CoponId)
        {
            var copon = await _context.Copon.Include(x => x.CoponRentalCompanies).FirstOrDefaultAsync(x => x.Id == CoponId);
            return copon;
        }

        public async Task<bool> EditCopon(int id, CoponCreateViewModel createCoponViewModel)
        {
            var foundedCopon = await _context.Copon.Include(x => x.CoponRentalCompanies).FirstOrDefaultAsync(x => x.Id == id);

            if (foundedCopon is null)
                return false;

            foundedCopon.CoponRentalCompanies = createCoponViewModel.RentalCompaniesIdsList
                .Select(x => new CoponRentalCompany
                {
                    RentalCompanyId = x.ToString(),
                }).ToList();

            foundedCopon.Count = (int)createCoponViewModel.Count;
            foundedCopon.Discount = (double)createCoponViewModel.Discount;
            foundedCopon.IsActive = createCoponViewModel.IsActive;
            foundedCopon.Expirdate = (DateTime)createCoponViewModel.expirdate;
            foundedCopon.limtDiscount = (double)createCoponViewModel.limt_discount;
            //foundedCopon.RentalCompanyId = createCoponViewModel.RentalCompanyId;

            _context.Update(foundedCopon);

            return await _context.SaveChangesAsync() > 0;
        }

        public bool IsExist(string CoponCode)
        {
            bool IsExist = _context.Copon.Where(x => x.CoponCode == CoponCode).Any();
            return IsExist;
        }
        public bool IsExist(int? CoponId)
        {
            bool IsExist = _context.Copon.Where(x => x.Id == CoponId).Any();
            return IsExist;
        }
        public async Task<bool> ChangeState(int? id)
        {
            var copon = _context.Copon.Find(id);
            if (copon.IsActive)
            {
                copon.IsActive = false;
            }
            else
            {
                copon.IsActive = true;
            }
            await _context.SaveChangesAsync();
            return copon.IsActive;
        }

        public async Task<bool> DeleteCopons(int? id)
        {
            var copon = await _context.Copon.FindAsync(id);
            copon.IsDeleted = true;
            await _context.SaveChangesAsync();

            return copon.IsDeleted;
        }
        public async Task<List<SelectListItem>> GetRentalCompany(string id = null, string lang = "ar")
        {
            var rentalsCompanies = await _context.Users
                  .Where(c => c.IsActive && c.IsAppravel && c.TypeUser == (int)UserType.RentalCompany)
                  .Select(c => new SelectListItem
                  {
                      Value = c.Id,
                      Text = c.user_Name,
                      Selected = id != null ? id == c.Id : false
                  }).ToListAsync();
            return rentalsCompanies;
        }

    }
}
