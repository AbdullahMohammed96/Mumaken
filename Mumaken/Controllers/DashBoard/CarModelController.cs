using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.CarTable;
using Mumaken.Domain.Entities.Location;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.CarModel)]
    public class CarModelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public CarModelController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var carModel = await _context.CarModels
                .OrderByDescending(c => c.Id)
                .Include(c => c.CarType)
                .ToListAsync();
            return View(carModel);
        }
        public async Task<IActionResult> Create()
        {
            var lang = Helper.GetLanguage();
            var carTypes = await _context.CarTypes
                 .Where(c => c.IsActive)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.ChangeLangName(lang),
                 })
                 .ToListAsync();
            ViewBag.carType = carTypes;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarModel carModel)
        {
            if (ModelState.IsValid)
            {
                carModel.IsActive = true;
                carModel.Date = DateTime.Now;
                _context.Add(carModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carModel);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var lang = Helper.GetLanguage();
            var carModel = await _context.CarModels.FindAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }
            var carTypes = await _context.CarTypes
                         .Where(c => c.IsActive)
                         .Select(c => new SelectListItem
                         {
                             Value = c.Id.ToString(),
                             Text = c.ChangeLangName(lang),
                             Selected = c.Id == carModel.CarTypeId
                         })
                         .ToListAsync();
            ViewBag.carType = carTypes;
            return View(carModel);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CarModel carModel)
        {
            var lang = Helper.GetLanguage();
            if (id != carModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                _toastNotification.AddSuccessToastMessage(lang == "ar" ? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return View(carModel);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            var carModel = await _context.CarModels.FindAsync(id);

            carModel.IsActive = !carModel.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { key = 1, data = carModel.IsActive });
        }
    }
}
