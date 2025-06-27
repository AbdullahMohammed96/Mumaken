namespace Mumaken.Infrastructure.Extension
{
    public class AuthorizeRolesAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Mumaken.Domain.Enums.Roles[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
