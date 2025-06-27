using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using NToastNotify;

namespace Mumaken.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationDbUser> _signInManager;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;
        private readonly IHelper _uploadImage;

        private readonly IToastNotification _toastNotification;
        public RegisterModel(
            UserManager<ApplicationDbUser> userManager,
            SignInManager<ApplicationDbUser> signInManager,
            ILogger<RegisterModel> logger,
            //IEmailSender emailSender,
            IWebHostEnvironment environment, ApplicationDbContext context, IHelper uploadImage, IToastNotification toastNotification)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;
            _environment = environment;
            _context = context;
            _uploadImage = uploadImage;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "RequiredEmail")]
            [EmailAddress(ErrorMessage = "InvalidEmail")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            //[Required(ErrorMessage = "من فضلك ادخل اسم المستخدم")]
            //public string FullName { get; set; }

            [Required(ErrorMessage = "RequiredPhoto")]
            [Display(Name = "Img")]
            public IFormFile Img { get; set; }

            public string PhotoPath { get; set; }

            [Required(ErrorMessage = "OldPassword")]
            [StringLength(100, ErrorMessage = "ValidPassword", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "ComparePassword")]
            public string ConfirmPassword { get; set; }


        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        //private string ProcessUploadedFile(IFormFile Photo, string Place)
        //{
        //    string uniqueFileName = null;
        //    if (Photo != null)
        //    {
        //        string uploadsFolder = Path.Combine(_environment.WebRootPath, $"images/{Place}");
        //        uniqueFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(Photo.FileName);
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            Photo.CopyTo(fileStream);
        //        }
        //    }
        //    return Helper.HelperMethods.BaisUrlHoste + "images/Users/" + uniqueFileName;
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var lang = Helper.GetLanguage();
                string uniqueFileName = null;
                if (Input.Img != null)
                {
                    if (!Helper.isImage(Input.Img))
                    {
                        _toastNotification.AddErrorToastMessage("من فضلك ادخل صوره ", new ToastrOptions { Title = "" });
                        return Page();
                    }
                    uniqueFileName = _uploadImage.Upload(Input.Img, (int)FileName.Users);
                    //ProcessUploadedFile(_environment, Input.Img, FoldersName.Users.ToString());
                }

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _userManager.FindByIdAsync(userId);

                var user = new ApplicationDbUser { UserName = Input.Email, 
                    Email = Input.Email, 
                    TypeUser = (int)UserType.Admin, 
                    ImgProfile = uniqueFileName, user_Name = Input.Email,
                    IsActive = true, 
                    ActiveCode = true ,
                    PhoneCode="",
                    PhoneNumber="",
                    CityId = 1
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    _toastNotification.AddSuccessToastMessage(lang=="ar"? "تم الاضافه بنجاح" : "", new ToastrOptions { Title = "" });
                    return LocalRedirect(returnUrl);
                }
               
                foreach (var error in result.Errors)
                {
                    if (error.Description.Contains("already taken") && error.Description.Contains("Username"))
                    {
                        ModelState.AddModelError(string.Empty, lang=="ar"? "هذا الايميل مستخدم من قبل" : "This email has already been used");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
