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

namespace Mumaken.Domain.DTO.AuthDTO
{
    public class ConfirmCodeAddDto
    {
        public string lang { get; set; } = "ar";
        public string userId { get; set; }
        public int code { get; set; }


    }
    public class ConfirmCodeDtoValidator : AbstractValidator<ConfirmCodeAddDto>
    {
        public ConfirmCodeDtoValidator(IStringLocalizer<ConfirmCodeAddDto> localizer)
        {
            RuleFor(x => x.code).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "code"), Helper.getFileTranslate(Lang.Translate.en, "code")));

            RuleFor(x => x.userId).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "userId"), Helper.getFileTranslate(Lang.Translate.en, "userId")));

        }

    }
}
