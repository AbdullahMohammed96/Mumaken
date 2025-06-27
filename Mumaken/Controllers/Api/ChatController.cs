using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.ChatDTO;
using Mumaken.Domain.Entities.Chat;
using Mumaken.Domain.Enums;
using Mumaken.Service.Api.Contract.Chat;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mumaken.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "ChatApi")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IChatService _chatServices;
        private readonly IHelper _helper;
        public ChatController(IChatService chatServices, IHelper helper)
        {
            _chatServices = chatServices;
            _helper = helper;
        }

        //دى هتجيب كل الشاتات الخاصه بشخص معين 
        [HttpPost(ApiRoutes.Chat.ListUsersMyChat)]
        public async Task<IActionResult> ListUsersMyChat(string lang = "ar")
        {
            try
            {
                string UserId = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;

                //List<ListUsersMyChatDto> ListUsers = await _chatServices.GetListUsersMyChat(UserId, lang);

                return Json(new { key = 1, /*data = ListUsers*/ });
            }
            catch (Exception ex)
            {
                return Json(new { key = 0, data = ex.Message });
            }
        }

        // دى هتجيب كل الرسائل الى فى شات معين هنا كان الشات على الطلب لو اللوجيك على حاجه تانيه خلاف الطلب نظبطها للوجيك الى انا عاوزه
        [HttpPost(ApiRoutes.Chat.ListMessagesUser)]
        public async Task<ActionResult> ListMessagesUser(int OrderId, int pageNumber = 1)
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
            List<ListMessageTwoUsersDto> ListMessages = await _chatServices.GetListMessageTwoUsersDto(userId, OrderId, pageNumber);

            return Json(new { key = 1, data = ListMessages });
        }

        // دى علشان الموبايل يقدر يرفع اى ملف فى الشات الفكره هنا انه بيرفع الملف عندى سواء كان صورة فيديو ...هرجعله لينك هيحطه عندى فى ال الرسائل عندى
        [HttpPost(ApiRoutes.Chat.UploadNewFile)]
        public ActionResult UploadFile([FromForm] UploadFileDto file)
        {
            string fileName = _helper.Upload(file.File, (int)FileName.ChatFiles);

            return Json(new { key = 1, data = fileName, msg = "تم الارسال" });
        }


    }
}
