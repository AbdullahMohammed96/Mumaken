using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.UserTables;
using Mumaken.Domain.Enums;
using Mumaken.Domain.ViewModel.Admin;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.AdminInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Service.DashBoard.Implementation.AdminImplementation
{
    public class AdminServices : IAdminServices
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationDbUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHelper _helper;

        public AdminServices(RoleManager<IdentityRole> roleManager, UserManager<ApplicationDbUser> userManager, ApplicationDbContext context, IHelper helper)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _helper = helper;
        }

        public async Task<bool> EditUsersInRoles(UserRolesViewModel obj)
        {
            foreach (var userId in obj.users)
            {
                var user = await _userManager.FindByIdAsync(userId);
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Count == 0)
                {
                    foreach (var roleId in obj.roles)
                    {
                        var roleFound = await _roleManager.FindByIdAsync(roleId);
                        await _userManager.AddToRoleAsync(user, roleFound.Name);
                    }
                }
                else
                {
                    foreach (var userRole in userRoles)
                    {
                        var userFound = await _userManager.FindByIdAsync(userId);
                        await _userManager.RemoveFromRoleAsync(userFound, userRole);
                    }

                    foreach (var roleId in obj.roles)
                    {
                        var roleFound = await _roleManager.FindByIdAsync(roleId);
                        await _userManager.AddToRoleAsync(user, roleFound.Name);
                    }
                }
            }

            return true;
        }

        public async Task<GetUsersWithRolesViewModel> GetUsersWithRoles()
        {
            var users = await _userManager.Users.Where(u => u.TypeUser == (int)UserType.Admin).ToListAsync();

            var rolesList = new List<string>();
            var roles = "";


            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var userRole in userRoles)
                {
                    roles += _helper.GetRole(userRole, "ar") + ", ";
                }
                rolesList.Add(roles);
                roles = "";
            }

            return new GetUsersWithRolesViewModel { users = users, roles = rolesList };
        }

        public async Task<List<RolesViewModel>> ListRoles(string lang="ar")
        {
            return await _roleManager.Roles.Where(role => role.Name != Roles.Mobile.ToString()).Select(x => new RolesViewModel {id= x.Id, name = _helper.GetRole(x.Name, lang) }).ToListAsync();
        }

        public async Task<List<ApplicationDbUser>> ListUsers()
        {
            return await _userManager.Users.Where(u => u.TypeUser == (int)UserType.Admin).ToListAsync();
        }
        
        public async Task<UserWithRolesViewModel> EditUserRoles(UserIdViewModel userId)
        {
            var user = await _userManager.FindByIdAsync(userId.id);
            var userRoles = await _userManager.GetRolesAsync(user);

            var userRoleList = new List<IdentityRole>();

            foreach (var userRole in userRoles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(userRole);
                if (roleExist)
                {
                    var role = _roleManager.Roles.FirstOrDefault(r => r.Name == userRole);
                    userRoleList.Add(role);
                }
            }

            return new UserWithRolesViewModel { user= user, userRoles = userRoleList };
        }


    }
}
