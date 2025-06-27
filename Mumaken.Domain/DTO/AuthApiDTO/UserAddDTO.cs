using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class UserAddDTO
    {
        public IFormFile imageProfile { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string phoneCode { get; set; }
        public DateTime birthDate { get; set; }
        public int cityId { get; set; }
        public int AddUserType { get; set; }
        public string password { get; set; }
        public string deviceId { get; set; }
        public string deviceType { get; set; }
        /// <summary>
        /// for title of notification
        /// </summary>
        public string projectName { get; set; }

        /// <summary>
        ///ar or en
        /// </summary>

        public string lang { get; set; } = "ar";

    }
    public class RegisterDtoValidator : AbstractValidator<UserAddDTO>
    {
        public RegisterDtoValidator(IStringLocalizer<UserAddDTO> localizer)
        {

            RuleFor(x => x.userName).NotEmpty()
           .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "userName"), Helper.getFileTranslate(Lang.Translate.en, "userName")));

            RuleFor(x => x.email).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "email"), Helper.getFileTranslate(Lang.Translate.en, "email")));

            RuleFor(x => x.phone).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "phone"), Helper.getFileTranslate(Lang.Translate.en, "phone")));

            RuleFor(x => x.password).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "password"), Helper.getFileTranslate(Lang.Translate.en, "password")))
                .Length(6, 100)
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "length"), Helper.getFileTranslate(Lang.Translate.en, "length")));

            RuleFor(x => x.deviceId).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "deviceId"), Helper.getFileTranslate(Lang.Translate.en, "deviceId")));

        }


    }

    //////////////////////////////////////////////////////

    //public class RegisterDtoValidator : AbstractValidator<UserAddDTO>
    //{
    //    public RegisterDtoValidator()
    //    {

    //        RuleFor(x => x.userName).NotEmpty().WithMessage(x => x.lang == "ar" ? "من فضلك ادخل الاسم" : "userName Is Required");

    //        RuleFor(x => x.email).NotEmpty().WithMessage(x => x.lang == "ar" ? "من فضلك ادخل الاميل" : "email Is Required")
    //                             .EmailAddress().WithMessage(x => x.lang == "ar" ? "ادخل صيغة اميل صحيحة" : "enter email in correct format");

    //        RuleFor(x => x.phone).NotEmpty().WithMessage(x => x.lang == "ar" ? "من فضلك ادخل رقم الجوال" : "phone is required");

    //        RuleFor(x => x.password).NotEmpty().WithMessage(x => x.lang == "ar" ? "من فضلك ادخل كلمة المرور" : "password is required")
    //                                .Length(6, 100).WithMessage(x => x.lang == "ar" ? "يجب ان يزيد كلمة المرور عن 6 أرقام" : "password must above 6 numbers");

    //        RuleFor(x => x.deviceId).NotEmpty().WithMessage(x => x.lang == "ar" ? "من فضلك ادخل ال DeviceId" : "DeviceId is required");

    //    }


    //}



}
