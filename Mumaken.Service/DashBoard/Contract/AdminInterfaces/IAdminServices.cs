using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Contract.AdminInterfaces
{
    public interface IAdminServices
    {
        Task<bool> EditUsersInRoles(UserRolesViewModel obj);
        Task<GetUsersWithRolesViewModel> GetUsersWithRoles();
        Task<List<ApplicationDbUser>> ListUsers();
        Task<List<RolesViewModel>> ListRoles(string lang = "ar");
        Task<UserWithRolesViewModel> EditUserRoles(UserIdViewModel userId);
    }
}
