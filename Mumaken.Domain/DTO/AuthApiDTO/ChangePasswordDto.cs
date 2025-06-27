using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.DTO.AuthApiDTO
{
    public class ChangePasswordDto
    {
        public string userId { get; set; }
        public string oldPassword { get; set; }
        [MinLength(6)]
        public string newPassword { get; set; }

        public string lang { get; set; } = "ar";

       
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator(IStringLocalizer<ChangePasswordDto> localizer)
        {



            RuleFor(x => x.oldPassword).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "oldPassword"), Helper.getFileTranslate(Lang.Translate.en, "oldPassword")))
                .Length(6, 100)
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "length"), Helper.getFileTranslate(Lang.Translate.en, "length")));

            RuleFor(x => x.newPassword).NotEmpty()
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "newPassword"), Helper.getFileTranslate(Lang.Translate.en, "newPassword")))
                .Length(6, 100)
                .WithMessage(e => AAITHelper.HelperMsg.creatMessage(e.lang, Helper.getFileTranslate(Lang.Translate.ar, "length"), Helper.getFileTranslate(Lang.Translate.en, "length")));
        }

    }
}
