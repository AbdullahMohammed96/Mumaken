using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Common.PathUrl;
using Mumaken.Domain.DTO.UserIndexDto;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using Mumaken.Domain.Common.Helpers.DataTablePaginationServer;
using Mumaken.Service.Api.Contract.Auth;
using Mumaken.Domain.ViewModel.Home;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.DeliveryCompany)]
    public class DeliveryCompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAppService _appService;
        private readonly IUserServices _userServices;
        private readonly IAccountService _accountService;
        public DeliveryCompanyController(ApplicationDbContext context, IAppService appService, IUserServices userServices, IAccountService accountService)
        {
            _context = context;
            _appService = appService;
            _userServices = userServices;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LoadData()
        {
            try
            {
                var lang = Helper.GetLanguage();
                // get data of HttpRequest
                var outf = Request.OutFun();

                // customer data As querable
                var clients = _context.Users
                    .Include(c => c.city)
                    .Where(tempcustomer => tempcustomer.TypeUser == (int)UserType.DeliverCompany&& tempcustomer.IsAppravel)
                    .OrderByDescending(o => o.PublishDate)
                    .Select(tempcustomer => new DeliveryCompanyIndexViewModel
                    {
                        Id = tempcustomer.Id,
                        Name = tempcustomer.user_Name,
                        PhoneNumber = tempcustomer.PhoneCode + " " + tempcustomer.PhoneNumber,
                        ImgProfile = DashBordUrl.baseUrlHost + tempcustomer.ImgProfile,
                        City = lang == "ar" ? tempcustomer.city.NameAr : tempcustomer.city.NameEn,
                        CityId=tempcustomer.city.Id,
                        CommercialRegisterNumber =tempcustomer.CommercialRegisterNumber,
                        IsActive = tempcustomer.IsActive,
                    }).AsNoTracking();


                // get data after filter
                var data = _appService.GetData<DeliveryCompanyIndexViewModel>(outf, clients, m =>
                m.Name.Contains(outf.searchValue) ||
                m.PhoneNumber.Contains(outf.searchValue) ||
                m.CommercialRegisterNumber.Contains(outf.searchValue) ||
                m.City.Contains(outf.searchValue));

                //Returning Json Data
                return Json(new { draw = outf.draw, recordsFiltered = outf.recordsTotal, recordsTotal = outf.recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IActionResult> NotApprovalDeliveryCompany()
        {
            var lang = Helper.GetLanguage();
            var companies=await _context.Users
                .Include(c=>c.city)
                .Include(c=>c.Distract)
                .Where(c => c.TypeUser == (int)UserType.DeliverCompany && c.IsAppravel == false)
                .Select(c => new NotApprovalDeliveryCompanyIndexViewModel
                {
                    Id = c.Id,
                    Name = c.user_Name,
                    PhoneNumber = c.PhoneCode + " " + c.PhoneNumber,
                    ImgProfile = DashBordUrl.baseUrlHost + c.ImgProfile,
                    City = lang == "ar" ? c.city.NameAr : c.city.NameEn,
                    Dictract=lang == "ar" ? c.Distract.NameAr : c.Distract.NameEn,
                    CommercialRegisterNumber = c.CommercialRegisterNumber
                }).ToListAsync();
            return View(companies);
        }
        public async Task<IActionResult> AccptRequestJoin(string id)
        {
            var result=await _userServices.AcceptRequestJoin(id, Helper.GetLanguage());
            return Json(new { key = 1, msg = result.Message });
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
        public async Task<IActionResult> GetDeliveryCompanyDriver(string id)
        {
            var result=await _accountService.GetDeliveryCompanyCaptains(id, Helper.GetLanguage());
            return View(result.Data);
        }
    }
}
