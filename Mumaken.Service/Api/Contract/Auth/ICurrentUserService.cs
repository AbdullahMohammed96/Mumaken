using Mumaken.Domain.Entities.UserTables;

namespace Mumaken.Service.Api.Contract.Auth
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        ApplicationDbUser user { get; }
    }
}
