using AAITHelper.Enums;
using AAITHelper;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.AuthApiDTO;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mumaken.Domain.DTO.AuthDTO;
using Microsoft.EntityFrameworkCore;

namespace Mumaken.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "UserAPI")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserServices _userService;

        public UserController(ICurrentUserService currentUserService, IUserServices userServices)
        {
            _currentUserService = currentUserService;
            _userService = userServices;
        }

        /// <summary>
        /// get user data
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
        /// <param lang="ar">ar or en</param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpGet(ApiRoutes.Identity.GetDataOfUser)]
        public async Task<IActionResult> GetDataOfUser(string lang = "ar")
        {

            var response = await _userService.GetUserInfo(_currentUserService.UserId, lang, "");
            return StatusCode(response.ResponseCode, response);
        }


        /// <summary>
        ///update user data 
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
        /// <param lang="userModel.lang">ar or en</param>
        /// <param userName="userModel.userName"></param>
        /// <param email="userModel.email"></param>
        /// <param phone="userModel.phone"></param>
        /// <param imgProfile="userModel.imgProfile"></param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpPut(ApiRoutes.Identity.UpdateDataUser)]
        public async Task<IActionResult> UpdateDataUser([FromForm] UpdateDataUserDto userModel)
        {
            userModel.lang = userModel.lang ?? "ar";
            var result=await _userService.UpdateDataUser(userModel);
            return StatusCode(result.ResponseCode, result);
        }
        [HttpGet(ApiRoutes.Identity.GetUserWalet)]
        public async Task<IActionResult> GetUserWalet()
        {

            var response = await _userService.GetUserWalet();
            return StatusCode(response.ResponseCode, response);
        }

        /// <summary>
        /// get provider data
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
        /// <param lang="ar">ar or en</param>
        /// <returns>return object of user </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>

        [HttpGet(ApiRoutes.Identity.GetDataOfProvider)]
        public async Task<IActionResult> GetDataOfProvider(string lang = "ar")
        {
            try
            {
                var response = await _userService.GetProviderInfo(_currentUserService.UserId, lang, "");
                if (response is not null)
                    return Ok(response);
                else
                {
                    return NotFound(HelperMsg.creatMessage(lang, "لم يتم العثور علي هذا المستخدم", "Not Found"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Change app Language 
        /// </summary>
        /// <returns>return status code </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpGet(ApiRoutes.Identity.RemoveAllNotify)]
        public async Task<IActionResult> RemoveAllNotify(string lang="ar")
        {
            var response = await _userService.DeleteAllNotify(lang);
            return StatusCode(response.ResponseCode, response);

        }
        [HttpGet(ApiRoutes.Identity.RemoveNotifyById)]
        public async Task<IActionResult> RemoveNotifyById(int notifyId, string lang = "ar")
        {
            var response = await _userService.DeleteNotifyById(notifyId, lang);
            return StatusCode(response.ResponseCode, response);

        }

        /// <summary>
        /// Close Notification for users
        /// </summary>
        /// <param lang="changeNotifyEditDto.lang">ar or en</param>
        /// <param notify="changeNotifyEditDto.notify"></param>
        /// <returns>return status code </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpPost(ApiRoutes.Identity.ChangeNotify)]
        public async Task<IActionResult> ChangeNotify([FromForm] NotifyDto changeNotifyEditDto)
        {
            changeNotifyEditDto.lang = changeNotifyEditDto.lang ?? "ar";

            if (await _userService.ChangeNotify(new ChangeNotifyEditDto()
            {
                notify = changeNotifyEditDto.notify,
                userId = _currentUserService.UserId,
                lang = changeNotifyEditDto.lang
            }))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// List Notification for Client
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///    {
        /// "id": "1",
        /// "text": any,
        /// "date": 18/10/2021,
        /// "type":1,
        ///  }
        /// </remarks>
        /// <param lang="ar">ar or en</param>
        /// <returns>return object of notify </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpGet(ApiRoutes.Identity.ListOfNotifyUser)]
        public async Task<IActionResult> ListOfNotifyUser(string lang = "ar")
        {
            var response = await _userService.ListOfNotifyUser(_currentUserService.UserId, lang);
            return StatusCode(response.ResponseCode, response);
        }

    }

}
