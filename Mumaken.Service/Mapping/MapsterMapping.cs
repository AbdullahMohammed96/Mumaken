using AAITHelper;
using Mapster;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.DTO.MoreDto;
using Mumaken.Domain.DTO.OrderDTO;
using Mumaken.Domain.Entities.AdditionalTables;
using Mumaken.Domain.Entities.CarTable;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Auth;
using Mumaken.Domain.ViewModel.Provider;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.Mapping
{
    public class MapsterMapping
    {
        public static void ConfigureMappings()
        {


            TypeAdapterConfig<UserAddDTO, ApplicationDbUser>
                .NewConfig()
                .Map(des => des.user_Name, sour => sour.userName)
                .Map(des => des.PhoneNumber, sour => sour.phone)
                .Map(des => des.PhoneCode, sour => sour.phoneCode)
                .Map(des => des.Email, sour => sour.email)
                .Map(des => des.ShowPassword, sour => sour.password)
                .Map(des => des.Lang, sour => sour.lang)
                .Map(des => des.PublishDate, sour => HelperDate.GetCurrentDate(3))
                .Map(des => des.SecurityStamp, sour => Guid.NewGuid().ToString())
                .Map(des => des.TypeUser, sour => (int)UserType.Client)
                .Map(des => des.UserName, sour => sour.phone + HelperNumber.GetRandomNumber() + "@yahoo.com")
                .Map(des => des.IsActive, sour => true)
                .Map(des => des.AddedUserType, sour => sour.AddUserType)
                .Map(des => des.BirthDate, sour => sour.birthDate)
                .Map(des => des.CityId, sour => sour.cityId);

            TypeAdapterConfig<UserAddDTO, DeviceIdAddDto>
              .NewConfig()
              .Map(des => des.deviceType, sour => sour.deviceType)
              .Map(des => des.deviceId, sour => sour.deviceId)
              .Map(des => des.projectName, sour => sour.projectName);


            TypeAdapterConfig<ApplicationDbUser, UserInfoDTO>.NewConfig()
             .Map(dest => dest.id, src => src.Id)
             .Map(dest => dest.imgProfile, src => DashBordUrl.baseUrlHost + src.ImgProfile)
             .Map(dest => dest.userName, src => src.user_Name)
             .Map(dest => dest.phone, src => src.PhoneNumber)
             .Map(dest => dest.phoneCode, src => src.PhoneCode)
             .Map(dest => dest.typeUser, src => src.TypeUser)
             .Map(dest => dest.email, src => src.Email)
             .Map(dest => dest.code, src => src.Code)
             .Map(dest => dest.ActiveCode, src => src.ActiveCode)
             .Map(dest => dest.closeNotify, src => src.CloseNotify)
             .Map(dest => dest.cityId, src => src.CityId)
             .Map(dest => dest.lang, src => src.Lang)
             .Map(des => des.registerDate, sour => sour.PublishDate)
             .Map(dest => dest.city, src => src.city.ChangeLangName(src.Lang));

            TypeAdapterConfig<ApplicationDbUser, ProviderInfoDto>
                .NewConfig()
             .Map(dest => dest.id, src => src.Id)
             .Map(dest => dest.imgProfile, src => DashBordUrl.baseUrlHost + src.ImgProfile)
             .Map(dest => dest.userName, src => src.user_Name)
             .Map(dest => dest.phone, src => src.PhoneNumber)
             .Map(dest => dest.phoneCode, src => src.PhoneCode)
             .Map(dest => dest.typeUser, src => src.TypeUser)
             .Map(dest => dest.email, src => src.Email)
             .Map(dest => dest.code, src => src.Code)
             .Map(dest => dest.ActiveCode, src => src.ActiveCode)
             .Map(dest => dest.closeNotify, src => src.CloseNotify)
             .Map(dest => dest.cityId, src => src.CityId)
             .Map(dest => dest.city, src => src.city.ChangeLangName(src.Lang))
             .Map(dest => dest.distractName, src => src.Distract.ChangeLangName(src.Lang))
             .Map(dest => dest.distractId, src => src.DistrictId)
             .Map(des => des.registerDate, sour => sour.PublishDate)
             .Map(dest => dest.lang, src => src.Lang)
             .Map(dest => dest.commercialRegisterNumber, src => src.CommercialRegisterNumber)
             .Map(dest => dest.location, src => src.Location)
             .Map(dest => dest.lat, src => src.Lat)
             .Map(dest => dest.lng, src => src.Lng);

            TypeAdapterConfig<OrderAddDto, Order>.NewConfig()
              .Map(src => src.GenderType, dest => dest.userInfoDto.genderType)
              .Map(src => src.NationalityId, dest => dest.userInfoDto.notionalityId)
              .Map(src => src.IdentityNumber, dest => dest.userInfoDto.identityNumber)
              //.Map(src => src.IdentityImage, dest => dest.userInfoDto.identityImage)
              //.Map(src => src.DrivingLicenseImage, dest => dest.userInfoDto.drivingLicenseImage)
              .Map(src => src.Age, dest => dest.userInfoDto.age)
              .Map(src => src.RentalPeriod, dest => dest.carInfo.rentalPeriod)
              .Map(src => src.DateRentalPeriod, dest => dest.carInfo.dateRentalPeriod)
              .Map(src => src.DateEndRentalPeriod, dest => dest.carInfo.dateRentalPeriod.AddDays(dest.carInfo.rentalPeriod))
              .Map(src => src.CoponCode, dest => dest.carInfo.CoponCode)
              .Map(src => src.TypePay, dest => dest.carInfo.TypePay)
              .Map(src => src.CreationDate, dest => DateTime.UtcNow.AddHours(3))
              .Map(src => src.OrderStatus, dest => (int)OrderStutes.NewOrder)
              .Map(src => src.OrderCase, dest => (int)OrderCase.WaitAcceptAdministration)
              .Map(src => src.IsPaid, dest => false);

            TypeAdapterConfig<Car, OrderCar>
               .NewConfig()
               .Ignore(dest => dest.Id)
               .Map(dest => dest.CarId, src => src.Id)
               .Map(dest => dest.CarNumber, src => src.CarNumber)
               .Map(dest => dest.RentalPriceDaily, src => src.RentalPriceDaily)
               //.Map(dest => dest.InsuranceInformation, src => src.InsuranceInformation)
               //.Map(dest => dest.FileInsurancInformation, src => src.FileInsurancInformation)
               //.Map(dest => dest.CarForm, src => src.CarForm)
               .Map(dest => dest.Note, src => src.Note)
               .Map(dest => dest.CarCategoryId, src => src.CarCategoryId)
               .Map(dest => dest.CarModelId, src => src.CarModelId)
               .Map(dest => dest.CarTypeId, src => src.CarTypeId)
               .Map(dest => dest.RentalCompanyId, src => src.RentalCompanyId);

            #region Provider
            TypeAdapterConfig<AddProviderViewModel, ApplicationDbUser>
           .NewConfig()
           .Map(des => des.user_Name, sour => sour.User_Name)
           .Map(des => des.PhoneNumber, sour => sour.PhoneNumber)
           .Map(des => des.PhoneCode, sour => sour.PhoneCode)
           .Map(des => des.Email, sour => sour.PhoneNumber + "gmail.com")
           .Map(des => des.ShowPassword, sour => sour.Password)
           .Map(des => des.Lang, sour => sour.Lang)
           .Map(des => des.PublishDate, sour => HelperDate.GetCurrentDate(3))
           .Map(des => des.SecurityStamp, sour => Guid.NewGuid().ToString())
           .Map(des => des.TypeUser, sour => (int)UserType.Client)
           .Map(des => des.UserName, sour => sour.PhoneNumber + HelperNumber.GetRandomNumber() + "@yahoo.com")
           .Map(des => des.IsActive, sour => true)
           .Map(des => des.TypeUser, sour => sour.UserType)
           .Map(des => des.IsAppravel, sour => false)
           .Map(des => des.CityId, sour => sour.CityId)
           .Map(des => des.DistrictId, sour => sour.DistractId);
            #endregion
        }
        public static ApplicationDbUser MapAddProviderViewModelToApplicationDbUser(AddProviderViewModel viewModel)
        {
            var user = new ApplicationDbUser
            {
                user_Name = viewModel.User_Name,
                PhoneNumber = viewModel.PhoneNumber,
                PhoneCode = viewModel.PhoneCode,
                CityId = viewModel.CityId,
                DistrictId = viewModel.DistractId,
                Location = viewModel.Location,
                Lat = viewModel.Lat,
                Lng = viewModel.Lng,
                CommercialRegisterNumber = viewModel.CommercialRegisterNumber,
                ShowPassword = viewModel.Password, // Note: Password should be handled securely
                TypeUser = viewModel.UserType,
                Lang = viewModel.Lang,
                IsActive = true,
                IsAppravel = false,
                Email = Guid.NewGuid() + viewModel.PhoneNumber + "@gmail.com",
                ActiveCode = false,
                PublishDate = HelperDate.GetCurrentDate(3),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid() + viewModel.PhoneNumber  + "@gmail.com",
                // Assign other properties here
            };

            return user;
        }
        public static ApplicationDbUser MapToNewCaptain(AddCapationViewModel model)
        {
            var captain = new ApplicationDbUser
            {
                user_Name = model.UserName,
                PhoneNumber = model.Phone,
                PhoneCode = model.PhoneCode,
                CityId = model.CityId,
                ShowPassword = model.Password,
                Email = model.Email,
                //Age = model.Age,
                BirthDate=model.BirthDate,
                NantionalityId = model.NationlityId,
                IdentityNumber = model.IdentityNumber,
                DeliverCompanyId = model.DeliverCompanyId,
                GenderType =model.GenderType,
                AddedUserType = (int)AddedUserType.AddDeliverCompany,
                IsActive = true,
                ActiveCode = true,
                TypeUser = (int)UserType.Client,
                PublishDate = HelperDate.GetCurrentDate(3),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid() + model.Phone + "@gmail.com",
                IsAppravel=false,
            };
            return captain;
        }
    }
}
