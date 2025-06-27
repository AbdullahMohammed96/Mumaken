using Mumaken.Domain.Common.PathUrl;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Service.Api.Contract.Logic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mumaken.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "OrderProvider")]
    [ApiController]
    public class OrderProviderController : ControllerBase
    {
        private readonly IOrderProvider _OrderProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountService _accountService;
        public OrderProviderController( ICurrentUserService currentUserService, IAccountService accountService, IOrderProvider OrderProvider)
        {
            _accountService = accountService;
            _OrderProvider = OrderProvider;
            _currentUserService = currentUserService;
        }
      


    }
}
