using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.CarTable;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Cars;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.DashBoard.Contract.CarInterfaces;
using NPOI.HSSF.Record.Chart;
using NToastNotify;

namespace Mumaken.RentalCompanyDashboard.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ICarServices _carServices;
        private readonly ICurrentUserService _currentUser;
        private readonly IToastNotification _toastNotification;
        public CarsController(ICarServices carServices, ICurrentUserService currentUser, IToastNotification toastNotification)
        {
            _carServices = carServices;
            _currentUser = currentUser;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _carServices.GetAllCar(_currentUser.user.Id, Helper.GetLanguage());
            return View(cars.Data);
        }
        [HttpGet]
        public async Task<IActionResult> AddCar()
        {
            var lang = Helper.GetLanguage();
            var currentUser = _currentUser.user.Id;
            ViewBag.currentUser = currentUser;
            var CarCategories = await _carServices.GetAllCarCatrgorySelectdList(null, lang);
            var cartypes = await _carServices.GetAllCarTypeSelectdList(null, lang);
            ViewBag.CarCategories = CarCategories;
            ViewBag.CarTypes = cartypes;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCar(AddCarViewModel model)
        {
            var lang = Helper.GetLanguage();
            if (ModelState.IsValid)
            {
                if (!Helper.isImage(model.CarImage))
                {
                    _toastNotification.AddErrorToastMessage(lang == "ar" ? "من فضلك صوره للسيارة" : "Please Enter Image To Car", new ToastrOptions { Title = "" });
                    ViewBag.currentUser = _currentUser.user.Id;
                    ViewBag.CarCategories = await _carServices.GetAllCarCatrgorySelectdList(null, lang);
                    ViewBag.CarTypes = await _carServices.GetAllCarTypeSelectdList(null, lang);
                    ViewBag.SelectCarModel = model.CarModelId;
                    return View(model);
                }
                model.lang = Helper.GetLanguage();
                var result = await _carServices.AddCar(model);
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            else
            {

                ViewBag.currentUser = _currentUser.user.Id;
                ViewBag.CarCategories = await _carServices.GetAllCarCatrgorySelectdList(null, lang);
                ViewBag.CarTypes = await _carServices.GetAllCarTypeSelectdList(null, lang);
                ViewBag.SelectCarModel = model.CarModelId;
                return View(model);

            }

        }
        public async Task<IActionResult> GetCarDetails(int carId)
        {
            var carDetails = await _carServices.CarDetails(carId, Helper.GetLanguage());
            return View(carDetails.Data);
        }
        [HttpGet]
        public async Task<IActionResult> EditCar(int carId)
        {
            var lang = Helper.GetLanguage();

            var carDetails = await _carServices.CarDetails(carId, lang);

            var CarCategories = await _carServices.GetAllCarCatrgorySelectdList(carDetails.Data.CarCategoryId, lang);
            var cartypes = await _carServices.GetAllCarTypeSelectdList(carDetails.Data.CarTypeId, lang);
            var models = await _carServices.GetAllCarModelWithTypeSelectdList(null, carDetails.Data.CarTypeId, Helper.GetLanguage());
            ViewBag.CarCategories = CarCategories;
            ViewBag.CarTypes = cartypes;
            ViewBag.models = models.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            var editModel = UpadateCarViewModel.MappDetailsCarToUpdateModel(carDetails.Data, lang);
            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditCar(UpadateCarViewModel model)
        {
            var lang = Helper.GetLanguage();
            model.lang = lang;
            if (ModelState.IsValid)
            {
                var result = await _carServices.UpdateCar(model);
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(EditCar), new { carId = model.Id });
        }
        [HttpGet]
        public async Task<IActionResult> GetCarModelWithType(int carTypeId)
        {
            var carModels = await _carServices.GetAllCarModelWithTypeSelectdList(null, carTypeId, Helper.GetLanguage());
            return Ok(carModels);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCar(int carId)
        {
            var delectCarResult = await _carServices.DeleteCar(carId, Helper.GetLanguage());
            if (delectCarResult.IsSuccess)
            {
                _toastNotification.AddSuccessToastMessage(delectCarResult.Message, new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            _toastNotification.AddSuccessToastMessage(delectCarResult.Message, new ToastrOptions { Title = "" });
            return RedirectToAction(nameof(GetCarDetails), new { carId = carId });
        }
    }
}
