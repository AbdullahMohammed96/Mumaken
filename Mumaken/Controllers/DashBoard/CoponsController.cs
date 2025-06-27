using Mumaken.Domain.Entities.Copon;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Copon;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.CoponsInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using NToastNotify;
using Mumaken.Domain.Common.Helpers;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin)]
    public class CoponsController : Controller
    {
        private readonly ICoponServices _coponServices;
        private readonly IToastNotification _toastNotification;
        public CoponsController(ICoponServices coponServices, IToastNotification toastNotification)
        {
            _coponServices = coponServices;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var copons = await _coponServices.GetCopons();
            return View(copons);
        }

        // GET: Copons/Create
        public async Task<IActionResult> Create()
        {
            var rentalCompanies = await _coponServices.GetRentalCompany();
            ViewBag.rentalCompanies = rentalCompanies;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoponCreateViewModel createCoponViewModel)
        {
            var rentalCompanies = await _coponServices.GetRentalCompany();
            if (ModelState.IsValid)
            {
                if (_coponServices.IsExist(createCoponViewModel.CoponCode))
                {
                    ModelState.AddModelError("CoponCode", "هذا الكود موجود من قبل");
                    return View(createCoponViewModel);
                }

                bool IsAdded = await _coponServices.CreateCopon(createCoponViewModel);
                _toastNotification.AddSuccessToastMessage(Helper.GetLanguage() == "ar" ? "تم الاضافة بنجاح" : "Added successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            ViewBag.rentalCompanies = rentalCompanies;
            return View(createCoponViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var copon = await _coponServices.GetCopon(id);
            if (copon == null)
            {
                return NotFound();
            }

            var createCoponViewModel = new CoponCreateViewModel()
            {
                Count = copon.Count,
                Id = copon.Id,
                //CountUsed = copon.CountUsed,
                Discount = copon.Discount,
                CoponCode = copon.CoponCode,
                IsActive = copon.IsActive,
                expirdate = copon.Expirdate,
                limt_discount = copon.limtDiscount,
                RentalCompaniesIds = string.Join(',', copon.CoponRentalCompanies.Select(x => x.RentalCompanyId)),
                RentalCompaniesIdsList = copon.CoponRentalCompanies.Select(x => x.RentalCompanyId).ToList()
                //RentalCompanyId = copon.RentalCompanyId,
            };

            var rentalCompanies = await _coponServices.GetRentalCompany();
            ViewBag.rentalCompanies = rentalCompanies;
            return View(createCoponViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CoponCreateViewModel createCoponViewModel)
        {
            if (id != createCoponViewModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (createCoponViewModel.expirdate.HasValue && createCoponViewModel.expirdate.Value <= DateTime.Now)
                    {
                        ModelState.AddModelError("expirdate", "يجب أن يكون تاريخ انتهاء الصلاحية أكبر من التاريخ الحالي");
                    }
                    await _coponServices.EditCopon(id, createCoponViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_coponServices.IsExist(createCoponViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _toastNotification.AddSuccessToastMessage(Helper.GetLanguage() == "ar" ? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return View(createCoponViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            bool IsActive = await _coponServices.ChangeState(id);

            return Ok(new { key = 1, data = IsActive });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            bool IsDeleted = await _coponServices.DeleteCopons(id);
            return Json(new { key = 1, data = IsDeleted });
        }

    }
}