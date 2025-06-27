using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.OrderCycleIntroScreen)]
    public class OrderCycleIntroScreenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public OrderCycleIntroScreenController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            //var introPages = await _context
            //    .OrderCycleIntros
            //    .OrderByDescending(c => c.Id).ToListAsync();
            var introPages = await _context.OrderCycleIntros
                        .Select(c => new OrderCycleIntro {
                            Id=c.Id,
                            TitleAr = Helper.RemoveHtmlTags(c.TitleAr),
                            TitleEn = Helper.RemoveHtmlTags(c.TitleEn),
                            DescriptionAr = Helper.RemoveHtmlTags(c.DescriptionAr),
                            DescriptionEn = Helper.RemoveHtmlTags(c.DescriptionEn),
                            IsActive=c.IsActive,
                            Index=c.Index
                        })
                .OrderByDescending(c => c.Id)
                .ToListAsync();
            return View(introPages);
        }
        public IActionResult Create()
        {
            return View();
        }
     
        [HttpPost]
        public async Task<IActionResult> Create(OrderCycleIntro introScreen)
        {
            if (ModelState.IsValid)
            {
                if (await _context.OrderCycleIntros.AnyAsync(o => o.Index == introScreen.Index))
                {
                    ModelState.AddModelError("Index", Helper.GetLanguage() == "ar" ? "الترتيب  موجود مسبقًا" : "Index already exists");
                    return View(introScreen);
                }

                if (introScreen.Index<=0)
                {
                    ModelState.AddModelError("Index", Helper.GetLanguage() == "ar" ? "ادخل الترتيب بطريقه صحيحة" : "Enter the Index correctly");
                    return View(introScreen);
                }
                introScreen.IsActive = true;
                _context.Add(introScreen);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage(Helper.GetLanguage() == "ar" ? "تم الاضافة بنجاح" : "Added successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return View(introScreen);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intoScreen = await _context.OrderCycleIntros.FindAsync(id);
            if (intoScreen == null)
            {
                return NotFound();
            }
            return View(intoScreen);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderCycleIntro introScreen)
        {
            if (id != introScreen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.OrderCycleIntros.AnyAsync(o => o.Index == introScreen.Index && o.Id != id))
                    {
                        ModelState.AddModelError("Index", Helper.GetLanguage() == "ar" ? "الترتيب موجود مسبقًا" : "Index already exists");
                        return View(introScreen);
                    }
                    if (introScreen.Index == 0)
                    {
                        ModelState.AddModelError("Index", Helper.GetLanguage() == "ar" ? "ادخل الترتيب بطريقه صحيحة" : "Enter the Index correctly");
                        return View(introScreen);
                    }
                    _context.Update(introScreen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                _toastNotification.AddSuccessToastMessage(Helper.GetLanguage() == "ar" ? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return View(introScreen);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            var intoScreen = await _context.OrderCycleIntros.FindAsync(id);

            intoScreen.IsActive = !intoScreen.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { key = 1, data = intoScreen.IsActive });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var intoScreen = await _context.OrderCycleIntros.FindAsync(id);
            _context.OrderCycleIntros.Remove(intoScreen);
            await _context.SaveChangesAsync();
            return Json(new { redirectToUrl = Url.Action("Index", "OrderCycleIntroScreen") });

        }
    }
}
