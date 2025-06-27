using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Region;
using Mumaken.Infrastructure.Extension;
using Mumaken.Service.DashBoard.Contract.LocationInterfaces;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Regoins)]
    public class RegionsController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IToastNotification _toastNotification;
        public RegionsController(ILocationService locationService, IToastNotification toastNotification)
        {
            _locationService = locationService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _locationService.GetAllRegion());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.city = await _locationService.GetCitiesSelectList(null,Helper.GetLanguage());
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegoinsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var lang = Helper.GetLanguage();
                    var result = await _locationService.AddRegion(model);

                    if (!result)
                    {

                        ViewBag.city = await _locationService.GetCitiesSelectList(null, lang);
                        return View(model);
                    }
                    _toastNotification.AddSuccessToastMessage(lang=="ar"?"تمت الاضافه بنجاح": "Added successfully", new ToastrOptions { Title = "" }) ;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ViewBag.city = await _locationService.GetCitiesSelectList(null, Helper.GetLanguage());
                    _toastNotification.AddErrorToastMessage(ex.Message, new ToastrOptions { Title = "" });
                    return View(model);
                }



            }
            ViewBag.city = await _locationService.GetCitiesSelectList(null, Helper.GetLanguage());
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            ViewBag.city = await _locationService.GetCitiesSelectList(Id);

            return View(await _locationService.GetRegion(Id.Value));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditRegoinViewModel model)
        {

            if (ModelState.IsValid)
            {
                var lang = Helper.GetLanguage();
                var result = await _locationService.UpdateRegion(model);
                if (result)
                {
                    _toastNotification.AddSuccessToastMessage(lang == "ar" ? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            return View(model);
        }
        public async Task<IActionResult> ChangeState(int? id)
        {
            return Json(new { data = await _locationService.ChangeRegionState(id.Value) });
        }
    }
}
