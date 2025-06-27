using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.Helpers.DataTablePaginationServer;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.UserIndexDto;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using Mumaken.Service.Api.Contract.Auth;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Clients)]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAppService _appService;
        private readonly IUserServices _userServices;
        public ClientsController(IAppService appService, ApplicationDbContext context, IUserServices userServices)
        {
            _appService = appService;
            _context = context;
            _userServices = userServices;
        }
        public IActionResult Index(DriverTypeEnum driverType = DriverTypeEnum.All)
        {
            @ViewBag.driverType = driverType;
            return View();
        }
        public async Task<IActionResult> LoadData(DriverTypeEnum driverType = DriverTypeEnum.All)
        {
            try
            {
                var lang = Helper.GetLanguage();
                // get data of HttpRequest
                var outf = Request.OutFun();

                // customer data As querable
                var clients = _context.Users
                    .Include(c => c.city)
                    .Where(tempcustomer => tempcustomer.TypeUser == (int)UserType.Client &&
                              (driverType == DriverTypeEnum.All || (int)tempcustomer.AddedUserType == (int)driverType))
                     .OrderBy(o => o.PublishDate)
                    .Select(tempcustomer => new UserIndexDto
                    {
                        Id = tempcustomer.Id,
                        UserName = tempcustomer.user_Name,
                        Email = tempcustomer.Email,
                        PhoneNumber = tempcustomer.PhoneCode + " " + tempcustomer.PhoneNumber,
                        ImgProfile = DashBordUrl.baseUrlHost + tempcustomer.ImgProfile,
                        City = lang == "ar" ? tempcustomer.city.NameAr : tempcustomer.city.NameEn,
                        CityId = tempcustomer.city.Id,
                        TypeUserText = Helper.GetTypeAddUserText(tempcustomer.AddedUserType, lang),
                        TypeUser = tempcustomer.AddedUserType,
                        IsActive = tempcustomer.IsActive,
                        CreationDate = tempcustomer.PublishDate
                    }).AsNoTracking();


                // get data after filter
                var data = _appService.GetData<UserIndexDto>(outf, clients, m =>
                m.UserName.Contains(outf.searchValue) ||
                m.PhoneNumber.Contains(outf.searchValue) ||
                m.Email.Contains(outf.searchValue)).OrderByDescending(c => c.CreationDate);

                //Returning Json Data
                return Json(new { draw = outf.draw, recordsFiltered = outf.recordsTotal, recordsTotal = outf.recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> ChangeState(string id)
        {
            var result = await _userServices.ChangeState(id);
            return Json(new { key = 1, data = result.Data });
        }
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var result = await _userServices.DeleteAccount(id, Helper.GetLanguage());
            if (result.IsSuccess)
            {
                return Json(new { key = 1, data = result.Data, msg = result.Message });
            }
            return Json(new { key = 0, data = result.Data, msg = result.Message });
        }
    }
}
