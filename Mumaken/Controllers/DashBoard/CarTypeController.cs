using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.CarTable;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    // ماركت السيارة
    [AuthorizeRoles(Roles.Admin, Roles.CarType)]
    public class CarTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public CarTypeController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CarTypes.OrderByDescending(c => c.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarType carType)
        {
            if (ModelState.IsValid)
            {
                carType.IsActive = true;
                carType.Date = DateTime.Now;
                _context.Add(carType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carType);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carType = await _context.CarTypes.FindAsync(id);
            if (carType == null)
            {
                return NotFound();
            }
            return View(carType);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarType cartype)
        {
            var lang=Helper.GetLanguage();
            if (id != cartype.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                _toastNotification.AddSuccessToastMessage(lang == "ar" ? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return View(cartype);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            var carType = await _context.CarTypes.FindAsync(id);

            carType.IsActive = !carType.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { key = 1, data = carType.IsActive });
        }
    }
}
