using AAITHelper.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.IntroScreens;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.IntroScreens)]
    public class IntroScreensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHelper _helper;
        private readonly IToastNotification _toastNotification;
        public IntroScreensController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, IHelper helper, IToastNotification toastNotification)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _helper = helper;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var splashScreens = await _context.SplashScreens
                .Select(c=>new SplashScreen
                {
                    Id = c.Id,
                    TitleAr = Helper.RemoveHtmlTags(c.TitleAr),
                    TitleEn = Helper.RemoveHtmlTags(c.TitleEn),
                    DescriptionAr = Helper.RemoveHtmlTags(c.DescriptionAr),
                    DescriptionEn = Helper.RemoveHtmlTags(c.DescriptionEn),
                    IsActive=c.IsActive,
                    ImgAr=c.ImgAr,
                    ImgEn = c.ImgEn
                })
                .OrderByDescending(c => c.Id)
                .ToListAsync();

            return View(splashScreens);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IntroScreenCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            


            var valid = _helper.ValidateImage(model.BackgroundImageAr);
            if (!string.IsNullOrWhiteSpace(valid))
            {
                ModelState.AddModelError("BackgroundImageAr", valid);

                return View(model);
            }
            var valid2 = _helper.ValidateImage(model.BackgroundImageEn);
            if (!string.IsNullOrWhiteSpace(valid2))
            {
                ModelState.AddModelError("BackgroundImageEn", valid);

                return View(model);
            }
            var intro = new SplashScreen
            {
                TitleAr = model.TitleAr,
                TitleEn = model.TitleEn,
                DescriptionAr = model.DetailsAr,
                DescriptionEn = model.DetailsEn,
                ImgAr = _helper.Upload(model.BackgroundImageAr, FileName.IntroApp.ToNumber()),
                ImgEn= _helper.Upload(model.BackgroundImageEn, FileName.IntroApp.ToNumber()),

                IsActive = true,
            };
            await _context.SplashScreens.AddAsync(intro);
            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("تم الاضافة بنجاح", new ToastrOptions { Title = "" });
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(byte id)
        {
            var intro = await _context.SplashScreens.Select(x => new IntroScreenEditViewModel
            {
                Id = x.Id,
                TitleAr = x.TitleAr,
                TitleEn = x.TitleEn,
                DetailsAr = x.DescriptionAr,
                DetailsEn = x.DescriptionEn,
                BackgroundAr= DashBordUrl.baseUrlHost + x.ImgAr,

                BackgroundEn = DashBordUrl.baseUrlHost+ x.ImgEn
            }).SingleOrDefaultAsync(x => x.Id == id);

            return View(intro);
        }



        [HttpPost]
        public async Task<IActionResult> Update(IntroScreenEditViewModel model)
        {
            var intro = await _context.SplashScreens.FindAsync(model.Id);
            if (intro is null)
                return NotFound($"No IntroScreen was found with id {model.Id}");

            if (!ModelState.IsValid)
                return View(model);

            intro.TitleAr = model.TitleAr;
            intro.TitleEn = model.TitleEn;
            intro.DescriptionAr = model.DetailsAr;
            intro.DescriptionEn = model.DetailsEn;

            if (model.NewBackgroundAr is not null)
            {


                var valid = _helper.ValidateImage(model.NewBackgroundAr);
                if (!string.IsNullOrWhiteSpace(valid))
                {
                    ModelState.AddModelError("BackgroundImageAr", valid);

                    return View(model);
                }


                intro.ImgAr = _helper.Upload(model.NewBackgroundAr, FileName.IntroApp.ToNumber());


            }

            if (model.NewBackgroundEn is not null)
            {


                var valid = _helper.ValidateImage(model.NewBackgroundEn);
                if (!string.IsNullOrWhiteSpace(valid))
                {
                    ModelState.AddModelError("BackgroundImageEn", valid);

                    return View(model);
                }


                intro.ImgEn = _helper.Upload(model.NewBackgroundEn, FileName.IntroApp.ToNumber());


            }
            _context.Update(intro);
            await _context.SaveChangesAsync();
            _toastNotification.AddSuccessToastMessage("تم التعديل بنجاح", new ToastrOptions { Title = "" });
            return RedirectToAction(nameof(Index));
        }





        [HttpPost]
        public IActionResult ChangeStatus(byte id)
        {
            var intro = _context.SplashScreens.Where(x => x.Id == id).SingleOrDefault();
            if (intro is null)
            {
                return Json(new { key = 0, msg = "No IntroScreen was found!!" });

            }

            intro.IsActive = !intro.IsActive;
            _context.SaveChanges();
            return Json(new { key = 1, data = intro.IsActive });
        }



        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var introscreen = await _context.SplashScreens.FindAsync(id);
            _context.SplashScreens.Remove(introscreen);
            await _context.SaveChangesAsync();

            return Json(new { redirectToUrl = Url.Action("Index", "IntroScreens") });
        }
    }
}
