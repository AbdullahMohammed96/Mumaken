// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Provider;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.More;
using NToastNotify;

namespace Mumaken.RentalCompanyDashboard.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IAccountService _accountService;
        private readonly IToastNotification _toastNotification;
        private readonly IMoreService _moreServices;

        public RegisterModel(
            UserManager<ApplicationDbUser> userManager,
            SignInManager<ApplicationDbUser> signInManager,
            ILogger<RegisterModel> logger,
            IAccountService accountService,
            IToastNotification toastNotification,
            IMoreService moreServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _accountService = accountService;
            _toastNotification = toastNotification;
            _moreServices = moreServices;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {
            [Required(ErrorMessage = "RequiedField")]
            public IFormFile ImageProfile { get; set; }
            [Required(ErrorMessage = "RequiedField")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "RequiedField")]
            //[RegularExpression("(05)(5|0|3|6|4|9|1|8|7)([0-9]{7})$",ErrorMessage = "CorrectPhoneNumber")]
            [RegularExpression("(5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$",ErrorMessage = "CorrectPhoneNumber")]
            public string PhoneNumber { get; set; }
            public string PhoneCode { get; set; }
            [Required(ErrorMessage = "RequiedField")]
            public int CityId { get; set; }
            [Required(ErrorMessage = "RequiedField")]
            public int DistractId { get; set; }
            public string Location { get; set; }
            public string Lat { get; set; }
            public string Lng { get; set; }
            [Required(ErrorMessage = "RequiedField")]
            [StringLength(10, ErrorMessage = "ValidCommercialRegisterNumber", MinimumLength = 10)]
            public string CommercialRegisterNumber { get; set; }
            public int UserType { get; set; }
            public string? Lang { get; set; }
            [Required(ErrorMessage = "RequiedField")]
            [StringLength(100, ErrorMessage = "ValidPassword", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required(ErrorMessage = "RequiedField")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "ConfirmPasswordAndPasswordCheck")]
            public string ConfirmPassword { get; set; }
         
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            var lang = Helper.GetLanguage();
            ViewData["Cites"] = await _moreServices.GetCitiesSelectList(null, lang);
            ViewData["AcceptConditionAndTerm"] = false;
            ViewData["lang"] = lang;
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var lang = Helper.GetLanguage();
            ViewData["lang"] = lang;
            if (ModelState.IsValid)
            {
                Input.Lang = lang;
                if (!Helper.isImage(Input.ImageProfile))
                {
                    _toastNotification.AddErrorToastMessage(lang=="ar"?"من فضلك ادخل صورة":"Please Enter Image", new ToastrOptions { Title = "" });
                    ViewData["Cites"] = await _moreServices.GetCitiesSelectList(null, lang);
                    ViewData["SelectDistract"] = Input.DistractId;
                    return Page();
                }
                var provider = MapInputModelToViewModel(Input);
                provider.UserType = (int)UserType.RentalCompany;
                var resultAddProvider = await _accountService.AddProvider(provider);

                if (resultAddProvider.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage(lang=="ar"? "تم إرسال كود التفعيل" : "Activation code has been sent", new ToastrOptions { Title = "" });
                    return RedirectToAction("ActiveCode", "DAuth", new { userId = resultAddProvider.Data.id });
                }
                _toastNotification.AddErrorToastMessage(resultAddProvider.Message, new ToastrOptions { Title = "" });
                ViewData["Cites"] = await _moreServices.GetCitiesSelectList(null, lang);
                ViewData["SelectDistract"] = Input.DistractId;
                return Page();
            }
            else
            {
                ViewData["Cites"] = await _moreServices.GetCitiesSelectList(null, lang);
                ViewData["SelectDistract"] = Input.DistractId;
                return Page();
            }

        }

        public static AddProviderViewModel MapInputModelToViewModel(InputModel input)
        {
            var viewModel = new AddProviderViewModel
            {
                ImageProfile = input.ImageProfile,
                User_Name = input.UserName,
                PhoneNumber = input.PhoneNumber,
                PhoneCode = input.PhoneCode,
                CityId = input.CityId,
                DistractId = input.DistractId,
                Location = input.Location,
                Lat = input.Lat,
                Lng = input.Lng,
                CommercialRegisterNumber = input.CommercialRegisterNumber,
                UserType = input.UserType,
                Lang = input.Lang,
                Password=input.Password,
                // Password and ConfirmPassword are not mapped as they are sensitive information
            };

            return viewModel;
        }
    }
}
