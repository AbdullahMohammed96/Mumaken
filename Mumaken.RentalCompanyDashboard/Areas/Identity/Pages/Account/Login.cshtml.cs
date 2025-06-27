// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.Enums;
using Mumaken.Domain.Common.Helpers;
using NToastNotify;

namespace Mumaken.RentalCompanyDashboard.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IAccountService _accountService;
        private readonly IToastNotification _toastNotification;
        public LoginModel(SignInManager<ApplicationDbUser> signInManager, ILogger<LoginModel> logger, IAccountService accountService, IToastNotification toastNotification)
        {
            _signInManager = signInManager;
            _logger = logger;
            _accountService = accountService;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
  
            public string PhoneCode { get; set; }
            [Required(ErrorMessage = "RequiedField")]
            [RegularExpression("(5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "CorrectPhoneNumber")]
            public string PhoneNumber { get; set; }
            [Required(ErrorMessage = "RequiedField")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var resultLogin = await _accountService.Login(new LoginDto
                {
                    type = (int)UserType.RentalCompany,
                    phone = Input.PhoneNumber,
                    password = Input.Password,
                    phoneCode= Input.PhoneCode,
                    lang= Helper.GetLanguage()
                });
                if (resultLogin.IsSuccess)
                {
                    if (!resultLogin.Data.ActiveCode)
                    {
                        _toastNotification.AddErrorToastMessage(resultLogin.Message, new ToastrOptions { Title = "" });
                        return RedirectToAction("ActiveCode", "DAuth", new { userId = resultLogin.Data.id });
                    }
                    var result = await _signInManager
                        .PasswordSignInAsync(await _accountService.GetUserUsingId(resultLogin.Data.id), Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Orders",new { ordercase =(int)RentalCompanyOrder.newOdrer});
                }
                else
                {
                    _toastNotification.AddErrorToastMessage(resultLogin.Message, new ToastrOptions { Title = "" });
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
