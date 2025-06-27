using AAITHelper;
using AAITHelper.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.Helpers.ResponseHelper;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO;
using Mumaken.Domain.DTO.SettingDto;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Cars;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.CarInterfaces;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Mumaken.Service.DashBoard.Implementation.CarImplementation
{
    public class CarServices : ICarServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IHelper _helper;
        public CarServices(ApplicationDbContext context, IHelper helper)
        {
            _context = context;
            _helper = helper;
        }

        public async Task<List<SelectListItem>> GetAllCarCatrgorySelectdList(int? carCategorySelected, string lang = "ar")
        {
            var carCategories = await _context.CarCategories
                 .Where(c => c.IsActive)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.ChangeLangName(lang)
                 }).ToListAsync();
            return carCategories;
        }
        public async Task<List<SelectListItem>> GetAllCarTypeSelectdList(int? carTypeSelected, string lang = "ar")
        {
            var carTypyes = await _context.CarTypes
                 .Where(c => c.IsActive)
                 .Select(c => new SelectListItem
                 {
                     Value = c.Id.ToString(),
                     Text = c.ChangeLangName(lang)
                 }).ToListAsync();
            return carTypyes;
        }
        public async Task<List<GeneralCarDataViewModel>> GetAllCarModelWithTypeSelectdList(int? carModelSelected, int typeId, string lang = "ar")
        {
            var carModels = await _context.CarModels.Where(c => c.IsActive && c.CarTypeId == typeId)
                 .Select(c => new GeneralCarDataViewModel
                 {
                     Id = c.Id,
                     Name = c.ChangeLangName(lang)
                 }).ToListAsync();
            return carModels;
        }
        public async Task<BaseResponseDto<object>> AddCar(AddCarViewModel model)
        {
            var newCar = AddCarViewModel.MappedToCar(model);
            newCar.CarImage =await _helper.UploadFileUsingApi(new UploadImageDto { image = model.CarImage, fileName=FileName.Car.ToNumber() });
           
            //newCar.CarForm =await _helper.UploadFileUsingApi(new UploadImageDto {image= model.CarForm, fileName=FileName.Car.ToNumber() });
            //newCar.FileInsurancInformation =await _helper.UploadFileUsingApi(new UploadImageDto { image = model.FileInsurancInformation,  fileName=FileName.Car.ToNumber() });
            try
            {
                await _context.Cars.AddAsync(newCar);
                await _context.SaveChangesAsync();
                return ResponseHelper.Success<object>(true,HelperMsg.creatMessage(model.lang,"تمت اضافه السيارة بنجاح", "The car has been added successfully"));
            }
            catch (Exception ex)
            {
                return ResponseHelper.Failure<object>(ex.Message);
            }

        }
        public async Task<BaseResponseDto<List<ListCarViewModel>>> GetAllCar(string rentalCompanyId, string lang = "ar")
        {
            var listCars = await _context.Cars
                .Include(c => c.CarCategory)
                .Include(c => c.CarModel)
                .Include(c => c.CarType)
                .Where(c => c.RentalCompanyId == rentalCompanyId)
                .Select(c => new ListCarViewModel
                {
                    Id = c.Id,
                    Image = DashBordUrl.baseUrlHost + c.CarImage,
                    CarNumber = c.CarNumber,
                    DailyPrice = c.RentalPriceDaily,
                    CarType = c.CarType.ChangeLangName(lang),
                    CarModel = c.CarModel.ChangeLangName(lang),
                    CarCategory = c.CarCategory.ChangeLangName(lang)
                }).ToListAsync();
            return ResponseHelper.Success(listCars);
        }
        public async Task<BaseResponseDto<GetCarDetails>> CarDetails(int carId, string lang = "ar")
        {
            var carDetails = await _context.Cars
                 .Where(c => c.Id == carId)
                 .Select(c => new GetCarDetails
                 {
                     Id = c.Id,
                     Image = DashBordUrl.baseUrlHost + c.CarImage,
                     CarNumber = c.CarNumber,
                     DailyPrice = c.RentalPriceDaily,
                     CarType = c.CarType.ChangeLangName(lang),
                     CarModel = c.CarModel.ChangeLangName(lang),
                     CarCategory = c.CarCategory.ChangeLangName(lang),
                     CarCategoryId=c.CarCategoryId,
                     CarModelId=c.CarModelId,
                     CarTypeId=c.CarTypeId,
                     Note = c.Note,
                     //InsuranceInformation = c.InsuranceInformation,
                     //CarForm = DashBordUrl.baseUrlHost + c.CarForm,
                     //FileInsurancInformation = DashBordUrl.baseUrlHost + c.FileInsurancInformation,
                 }).FirstOrDefaultAsync();
            return ResponseHelper.Success(carDetails);
        }
        public  async Task<BaseResponseDto<object>> UpdateCar(UpadateCarViewModel model)
        {
            var car=await _context.Cars.Where(c=>c.Id==model.Id).FirstOrDefaultAsync();
            car=UpadateCarViewModel.MappedToCar(model, car);
            if (model.NewImage is not null)
                car.CarImage = await _helper.UploadFileUsingApi(new UploadImageDto {image= model.NewImage, fileName=FileName.Car.ToNumber() });

            //if (model.NewCarForm is not null)
            //    car.CarForm =await _helper.UploadFileUsingApi(new UploadImageDto { image = model.NewCarForm, fileName=FileName.Car.ToNumber() });

            //if (model.NewFileInsurancInformation is not null)
            //    car.FileInsurancInformation = await _helper.UploadFileUsingApi(new UploadImageDto { image = model.NewFileInsurancInformation, fileName = FileName.Car.ToNumber() });

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return ResponseHelper.Success<object>(true,HelperMsg.creatMessage(model.lang,"تم تعديل السيارة بنجاح", "The car has been successfully modified"));
        }
        public async Task<BaseResponseDto<object>> DeleteCar(int carId, string lang = "ar")
        {
            //var checkExsistCarInOrder = _context.Orders
            //    .Where(c => c.OrderCar.CarId == carId && c.OrderStatus != (int)OrderStutes.Canceled && c.OrderStatus != (int)OrderStutes.Finished)
            //    .Any();
            var checkExsistCarInOrder = _context.OrderCars
                .Where(c =>c.CarId==carId&&c.Order.OrderStatus==(int)OrderStutes.Current&& c.Order.OrderStatus == (int)OrderStutes.NewOrder)
                .Any();
            if (checkExsistCarInOrder)
                return ResponseHelper.Failure<object>(HelperMsg.creatMessage(lang, "لا يمكنك حذف سياره في طلب ", "You cannot delete a Car in an order"));
          
            var deleletedCar = await _context.Cars.FirstOrDefaultAsync(c => c.Id == carId);
            deleletedCar.IsDeleted = true;


            _context.Update(deleletedCar);
            await _context.SaveChangesAsync();
            return ResponseHelper.Success<object>(true,HelperMsg.creatMessage(lang, "تم الحذف بنجاح", "Deleted successfully"));
        }
    }
}
