using Mumaken.Service.Api.Contract.Auth;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Mumaken.Domain.Entities.UserTables;

namespace Mumaken.Service.Api.Implementation.Auth
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationDbUser> _userManager;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationDbUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public string UserId => _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
        public ApplicationDbUser user => _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User).Result;
    }
}
