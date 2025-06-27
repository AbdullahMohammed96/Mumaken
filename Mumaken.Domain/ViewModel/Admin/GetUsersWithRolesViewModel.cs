using Mumaken.Domain.Entities.UserTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mumaken.Domain.ViewModel.Admin
{
    public class GetUsersWithRolesViewModel
    {
        public List<ApplicationDbUser> users { get; set; }
        public List<string> roles { get; set; }
    }
}
