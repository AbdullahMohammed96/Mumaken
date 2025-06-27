using Mumaken.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Mumaken.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static List<IdentityRole> IdentityRoleList()
        {
            List<IdentityRole> identityRoles = new List<IdentityRole>();
            foreach (Roles role in (Roles[])Roles.GetValues(typeof(Roles)))
            {
                identityRoles.Add(new IdentityRole(role.ToString()));
            }

            return identityRoles;
        }
    }
}
