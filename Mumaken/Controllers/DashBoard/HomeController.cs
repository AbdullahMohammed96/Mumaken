using AAITHelper.Enums;
using Mumaken.Domain.Enums;
using Mumaken.Domain.Model;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.HomeInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Mumaken.Controllers.DashBoard
{
    [Authorize]

    public class HomeController : Controller
    {
        private readonly IHomeServices _homeServices;

        public HomeController(IHomeServices homeServices = null)
        {
            _homeServices = homeServices;
        }

        public IActionResult Index()
        {
            var data =_homeServices.HomeIndex();

            return View(data);
        }
    }
}
