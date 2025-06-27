using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.CarTable;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.CarCategory)]
    public class CarCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public CarCategoryController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CarCategories.OrderByDescending(c => c.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarCategory carCategory)
        {
            if (ModelState.IsValid)
            {
                carCategory.IsActive = true;
                carCategory.Date = DateTime.Now;
                _context.Add(carCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carCategory);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carCategory = await _context.CarCategories.FindAsync(id);
            if (carCategory == null)
            {
                return NotFound();
            }
            return View(carCategory);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarCategory carCategory)
        {
            var lang = Helper.GetLanguage();
            if (id != carCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                _toastNotification.AddSuccessToastMessage(lang == "ar" ? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return View(carCategory);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            var carCategory = await _context.CarCategories.FindAsync(id);

            carCategory.IsActive = !carCategory.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { key = 1, data = carCategory.IsActive });
        }
    }
}
