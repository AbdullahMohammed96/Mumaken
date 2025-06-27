using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using Mumaken.Service.DashBoard.Contract.ContactUsInterfaces;
using Mumaken.Service.DashBoard.Implementation.ContactUsImplementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.ContactUs)]
    public class ContactUsController : Controller
    {
        private readonly IContactUsServices _contactUsServices;

        public ContactUsController(IContactUsServices contactUsServices)
        {
            _contactUsServices = contactUsServices;
        }

        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        // GET: ContactUs
        public async Task<IActionResult> Index()
        {
            return View(await _contactUsServices.GetContactUs());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            bool IsDeleted = await _contactUsServices.DeleteContactUs(id);
            return Json(new { key = 1, data = IsDeleted });
        }
    }
}
