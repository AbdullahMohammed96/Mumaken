using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Slider;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Sliders)]
    public class SlidersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IToastNotification _toastNotification;
        private readonly IHelper _helper;

        public SlidersController(ApplicationDbContext context, IToastNotification toastNotification, IHelper helper)
        {
            _context = context;
            _toastNotification = toastNotification;
            _helper = helper;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.OrderByDescending(c => c.Id).Select(c => new ListSliderViewModel
            {
                Id = c.Id,
                ImageAr = DashBordUrl.baseUrlHost + c.ImageAr,
                ImageEn = DashBordUrl.baseUrlHost + c.ImageEn,
                IsActive = c.IsActive,

            }).ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddSliderViewModel model)
        {
            try
            {
                var lang = Helper.GetLanguage();


                var valid = _helper.ValidateImage(model.ImgAr);
                if (!string.IsNullOrWhiteSpace(valid))
                {
                    ModelState.AddModelError("ImgAr", valid);

                    return View(model);
                }
                var valid2 = _helper.ValidateImage(model.ImgEn);
                if (!string.IsNullOrWhiteSpace(valid))
                {
                    ModelState.AddModelError("ImgEn", valid2);

                    return View(model);
                }

                var slider = new Slider
                {
                    ImageAr = _helper.Upload(model.ImgAr, (int)FileName.Slider),
                    ImageEn = _helper.Upload(model.ImgAr, (int)FileName.Slider),
                    IsActive = true,

                };
                _context.Sliders.Add(slider);
                await _context.SaveChangesAsync();
                _toastNotification.AddSuccessToastMessage(lang=="ar"? "تم الاضافة بنجاح" : "Add successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(model);
            }

        }
        public async Task<IActionResult> Edit(int? id)
        {
            var slider = await _context.Sliders.Where(c => c.Id == id).Select(c => new EditSliderViewModel
            {
                Id = c.Id,
                ImgAr = DashBordUrl.baseUrlHost + c.ImageAr,
                 ImgEn = DashBordUrl.baseUrlHost + c.ImageEn
            }).FirstOrDefaultAsync();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSliderViewModel model)
        {
            var lang = Helper.GetLanguage();


            var slider = await _context.Sliders.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (slider == null)
            {
                _toastNotification.AddErrorToastMessage(lang == "ar" ? "هذا السلايدر غير موجود" : "This slider not found", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
          
            if (model.NewImgAr != null)
            {
                var valid = _helper.ValidateImage(model.NewImgAr);
                if (!string.IsNullOrWhiteSpace(valid))
                {
                    ModelState.AddModelError("NewImgAr", valid);

                    return View(model);
                }

                slider.ImageAr = _helper.Upload(model.NewImgAr, (int)FileName.Slider);
            }
            if (model.NewImgEn != null)
            {
                var valid = _helper.ValidateImage(model.NewImgEn);
                if (!string.IsNullOrWhiteSpace(valid))
                {
                    ModelState.AddModelError("NewImgEn", valid);

                    return View(model);
                }
                slider.ImageEn = _helper.Upload(model.NewImgEn, (int)FileName.Slider);
            }

            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage(lang=="ar"? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ChangeState(int? id)
        {
            var slider = await _context.Sliders.Where(c => c.Id == id).FirstOrDefaultAsync();
            slider.IsActive = !slider.IsActive;
            await _context.SaveChangesAsync();
            return Json(new { data = slider.IsActive });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(c => c.Id == id);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return Json(new { redirectToUrl = Url.Action("Index", "Sliders") });

        }
    }
}
