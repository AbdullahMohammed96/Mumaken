using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Admin;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.AdminInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin)]

    public class AdminController : Controller
    {


        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }


        public IActionResult UserInRoles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRoles([FromBody] UserRolesViewModel obj)
        {
            var lang = Helper.GetLanguage();
            if (obj.users.Length == 0)
                return Json(new { key = 2, msg = lang=="ar"? "يجب تحديد مستخدم" : "You must select a user." });

            await _adminServices.EditUsersInRoles(obj);

            return Json(new { key = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var result=await _adminServices.GetUsersWithRoles();
            return Json(new { users = result.users, roles = result.roles });
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles([FromBody] UserIdViewModel userId)
        {
            var UserRoles=await _adminServices.EditUserRoles(userId);
            return Json(new { user= UserRoles.user, userRoles = UserRoles.userRoles });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var lang = Helper.GetLanguage();
            var roles = await _adminServices.ListRoles(lang);
            if (roles.Count() > 0)
            {
                return Json(new { key = 1, roles });
            }
            else
            {
                return Json(new { key = 0, msg =lang=="ar"? "لايوجد صلاحيات" : "There are no permissions" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAlluser()
        {
            var users = await _adminServices.ListUsers();
            var lang = Helper.GetLanguage();
            if (users.Count() > 0)
            {
                return Json(new { key = 1, users = users });
            }
            else
            {
                return Json(new { key = 0, msg = lang=="ar"? "لايوجد مستخدمين" : "There are no users" });
            }
        }

    }
}
