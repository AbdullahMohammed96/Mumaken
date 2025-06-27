using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class ForgetPasswordAddDto
    {
        //public string phone { get; set; }
        //public string phoneCode { get; set; }
        public string email { get; set; }
        public string lang { get; set; } = "ar";

       
    }
    public class ForgetPasswordDtoValidator : AbstractValidator<ForgetPasswordAddDto>
    {
        public ForgetPasswordDtoValidator(IStringLocalizer<ForgetPasswordAddDto> localizer)
        {
            //RuleFor(x => x.phone).NotEmpty()
            //               .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "phone"), Helper.getFileTranslate(Lang.Translate.en, "phone")));

            // .Matches(@"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$").WithMessage(x => x.lang == "ar" ? "من فضلك رقم الجوال بشكل صحيح" : "Enter phone in correct format");

        }

    }
}
