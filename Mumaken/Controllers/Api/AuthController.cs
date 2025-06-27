using AAITHelper;
using AAITHelper.Enums;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.DTO.AuthDTO;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Mumaken.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "AuthAPI")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IUserServices _userService;
        public AuthController(IConfiguration configuration, ICurrentUserService currentUserService, IAccountService accountService, IUserServices userServices)
        {
            _currentUserService = currentUserService;
            _accountService = accountService;
            _userService = userServices;
            _configuration = configuration;
        }

        /// <summary>
        /// Add new user 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   {
        /// "id": "c3eed5f3-ca71-4833-ace9-62ee3b608d95",
        /// "userName": "ahmed",
        /// "email": "ahmed@gmail.com",
        /// "phone": "01234567895",
        /// "lang": "ar",
        /// "closeNotify": false,
        /// "status": false,
        /// "imgProfile": "https://image.freepik.com/free-vector/user-icon_126283-435.jpg",
        /// "token": "",
        /// "typeUser": 1,
        /// "code": 1234,
        /// "activeCode": false
        ///  }
        /// </remarks>
        /// <param lang="UserAddDTO.lang">ar or en</param>
        /// <param userName="UserAddDTO.userName"></param>
        /// <param email="UserAddDTO.email"></param>
        /// <param phone="UserAddDTO.phone"></param>
        /// <param password="UserAddDTO.password"></param>
        /// <param deviceId="UserAddDTO.deviceId"> for Notify </param>
        /// <param deviceType="UserAddDTO.deviceType">ios or android </param>
        /// <param projectName="UserAddDTO.projectName">For title of notification </param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.RegisterClient)]
        public async Task<IActionResult> RegisterClient([FromForm] UserAddDTO model)
        {
            var result = await _accountService.AddUser(model);

            return StatusCode(result.ResponseCode, result);
        }

        /// <summary>
        /// Confirm Code for Register 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   {
        /// "id": "c3eed5f3-ca71-4833-ace9-62ee3b608d95",
        /// "userName": "ahmed",
        /// "email": "ahmed@gmail.com",
        /// "phone": "01234567895",
        /// "lang": "ar",
        /// "closeNotify": false,
        /// "status": false,
        /// "imgProfile": "https://image.freepik.com/free-vector/user-icon_126283-435.jpg",
        /// "token": "",
        /// "typeUser": 1,
        /// "code": 1234,
        /// "activeCode": false
        ///  }
        /// </remarks>
        /// <param lang="confirmCodeAddDto.lang">ar or en</param>
        /// <param userId="confirmCodeAddDto.Id"></param>
        /// <param code="confirmCodeAddDto.code"></param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.ConfirmCodeRegister)]
        public async Task<IActionResult> ConfirmCodeRegister([FromForm] ConfirmCodeAddDto confirmCodeAddDto)
        {
            var result = await _accountService.ConfirmCodeRegister(confirmCodeAddDto);
            return StatusCode(result.ResponseCode, result);
        }

        /// <summary>
        /// resend Code for Register 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   {
        /// "id": "c3eed5f3-ca71-4833-ace9-62ee3b608d95",
        /// "phone": "01234567895",
        /// "code": 1234,
        ///  }
        /// </remarks>
        /// <param lang="resendCodeAddDto.lang">ar or en</param>
        /// <param userId="resendCodeAddDto.Id"></param>
        /// <param code="resendCodeAddDto.code"></param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.ResendCode)]
        public async Task<IActionResult> ResendCode([FromForm] ResendCodeAddDto resendCodeAddDto)
        {
            resendCodeAddDto.lang = resendCodeAddDto.lang ?? "ar";

            var result = await _accountService.ResendCode(resendCodeAddDto);
            return StatusCode(result.ResponseCode, result);
        }

        /// <summary>
        /// LogIn
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   {
        /// "id": "c3eed5f3-ca71-4833-ace9-62ee3b608d95",
        /// "userName": "ahmed",
        /// "email": "ahmed@gmail.com",
        /// "phone": "01234567895",
        /// "lang": "ar",
        /// "closeNotify": false,
        /// "status": false,
        /// "imgProfile": "https://image.freepik.com/free-vector/user-icon_126283-435.jpg",
        /// "token": "",
        /// "typeUser": 1,
        /// "code": 1234,
        /// "activeCode": false
        ///  }
        /// </remarks>
        /// <param lang="LoginDto.lang">ar or en</param>
        /// <param phone="LoginDto.phone"></param>
        /// <param password="LoginDto.password"></param>
        /// <param deviceId="LoginDto.deviceId"> for Notify </param>
        /// <param deviceType="LoginDto.deviceType">ios or android </param>
        /// <param projectName="LoginDto.projectName">For title of notification </param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            var result = await _accountService.Login(loginDto);
            return StatusCode(result.ResponseCode, result);
        }

        /// <summary>
        /// Forget password
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   {
        /// "id": "c3eed5f3-ca71-4833-ace9-62ee3b608d95",
        /// "code": 1234,
        ///  }
        /// </remarks>
        /// <param lang="forgetPasswordDto.lang">ar or en</param>
        /// <param phone="forgetPasswordDto.phone"></param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.ForgetPassword)]
        public async Task<IActionResult> ForgetPassword([FromForm] ForgetPasswordAddDto forgetPasswordDto)
        {
            var result = await _accountService.ForgetUserPassword(forgetPasswordDto);
            return StatusCode(result.ResponseCode, result);
        }
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.CheckCode)]
        public async Task<IActionResult> CheckCode(string userId, int code, string lang)
        {
            var result = await _accountService.CheckCode(userId,code,lang);
            return StatusCode(result.ResponseCode, result);
        }

        /// <summary>
        /// Change user Password by code
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   "msg":"Password changed"
        /// </remarks>
        /// <param lang="changePasswordDto.lang">ar or en</param>
        /// <param phone="changePasswordDto.code"></param>
        /// <param phone="changePasswordDto.userId"></param>
        /// <param password="changePasswordDto.newPassword"></param>
        /// <returns>return msg </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.ChangePasswordByCode)]
        public async Task<IActionResult> ChangePasswordByCode([FromForm] ChangePasswordByCodeDto changePasswordByCodeDto)
        {
            var result = await _accountService.ChangePasswordByCode(changePasswordByCodeDto);
            return StatusCode(result.ResponseCode, result);
        }


        /// <summary>
        /// Change user Password
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   "msg":"Password changed"
        /// </remarks>
        /// <param lang="changePasswordDto.lang">ar or en</param>
        /// <param phone="changePasswordDto.oldPassword"></param>
        /// <param password="changePasswordDto.newPassword"></param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpPost(ApiRoutes.Identity.ChangePassward)]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordDto changePasswordDto)
        {
            var result = await _accountService.ChangePassword(changePasswordDto);
            return StatusCode(result.ResponseCode, result);
        }

        [HttpPost(ApiRoutes.Identity.ChangePhoneNumber)]
        public async Task<IActionResult> ChangePhoneNumber([FromForm] ChangePhoneDto model)
        {
            var result = await _accountService.ChangePhoneNumber(model);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpPost(ApiRoutes.Identity.ConfirmChangePhoneDto)]
        public async Task<IActionResult> ConfirmChangePhoneNumber([FromForm] ConfirmChangePhoneDto model)
        {
            var result = await _accountService.ConfirmChangePhoneNumber(model);
            return StatusCode(result.ResponseCode, result);
        }



        [HttpPost(ApiRoutes.Identity.ChangeEmail)]
        public async Task<IActionResult> ChangeEmail([FromForm] ChangeEmailDto model)
        {
            var result = await _accountService.ChangeEmail(model);
            return StatusCode(result.ResponseCode, result);
        }

        [HttpPost(ApiRoutes.Identity.ConfirmChangeEmail)]
        public async Task<IActionResult> ConfirmChangeEmail([FromForm] ConfirmChangeEmailDto model)
        {
            var result = await _accountService.ConfirmChangeEmail(model);
            return StatusCode(result.ResponseCode, result);
        }

        /// <summary>
        /// Change app Language 
        /// </summary>
        /// <param lang="ar">ar or en</param>
        /// <returns>return status code </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpPost(ApiRoutes.Identity.ChangeLanguage)]
        public async Task<IActionResult> ChangeLanguage([FromQuery] string lang = "ar")
        {
            var result = await _accountService.ChangeLanguage(lang);
            return StatusCode(result.ResponseCode, result);
        }
        /// <summary>
        /// user logout
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   "msg":"LogOut Successfully "
        /// </remarks>
        /// <param lang="userModel.lang">ar or en</param>
        /// <param deviceId="userModel.deviceId"></param>
        /// <returns>return msg </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpDelete(ApiRoutes.Identity.Logout)]
        public async Task<IActionResult> Logout([FromQuery] LogoutDto userModel)
        {
            var result = await _accountService.Logout(userModel);
            return StatusCode(result.ResponseCode, result);
        }


        /// <summary>
        /// Remove user account  مسح الحساب
        /// </summary>
        /// <remarks>
        ///  Sample request:
        ///  
        ///      {
        ///        "lang": "ar",   
        ///      }
        ///  
        /// </remarks>
        /// <param lang="ar">ar or en</param>
        /// <returns>return status code </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpDelete(ApiRoutes.Identity.RemoveAccount)]
        public async Task<IActionResult> RemoveAccount(string lang = "ar")
        {

            var result = await _accountService.RemoveAccount(_currentUserService.UserId,lang);
            return StatusCode(result.ResponseCode, result);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.SendSMS)]
        public async Task<ActionResult> SendSMS(int code, string phone)
        {
            try
            {
                string r = await _accountService.SendSms(code, phone);

                return Ok(new
                {
                    key = 0,
                    msg = r
                });

            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    key = 0,
                    msg = ex.Message
                });
            }
        }

    }
}
