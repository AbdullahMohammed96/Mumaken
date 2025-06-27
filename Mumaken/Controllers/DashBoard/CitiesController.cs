using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Region;
using Mumaken.Infrastructure.Extension;
using Mumaken.Service.DashBoard.Contract.LocationInterfaces;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Cities)]
    public class CitiesController : Controller
    {
        private readonly ILocationService _locationService;

        private readonly IToastNotification _toastNotification;

        public CitiesController(ILocationService locationService, IToastNotification toastNotification)
        {
            _locationService = locationService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var cities = await _locationService.GetAllCities();
            return View(cities);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _locationService.AddCity(model);

                if (result)
                {
                    _toastNotification.AddSuccessToastMessage(Helper.GetLanguage()=="ar"? "تم الاضافة بنجاح" : "Added successfully", new ToastrOptions { Title = "" });
                    return RedirectToAction("Index");
                }


                return View(model);
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            return View(await _locationService.GetCity(Id.Value));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCityViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _locationService.UpdateCity(model);
                if (result)
                {
                    _toastNotification.AddSuccessToastMessage(Helper.GetLanguage()=="ar"?"تم التعديل بنجاح": "Modified successfully", new ToastrOptions { Title = "" });
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            return View(model);
        }
        public async Task<IActionResult> ChangeState(int? id)
        {
            return Json(new { data = await _locationService.ChangeState(id.Value) });
        }
    }
}
