using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mumaken.Domain.Common.Helpers;
using Mumaken.Domain.Entities.SettingTables;
using Mumaken.Domain.Enums;
using Mumaken.Infrastructure.Extension;
using Mumaken.Persistence;
using NToastNotify;

namespace Mumaken.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin, Roles.Banks)]
    public class AdminBankAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public AdminBankAccountController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminBankAccounts.OrderByDescending(c => c.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdminBankAccount bank)
        {
            if (ModelState.IsValid)
            {
                var lang = Helper.GetLanguage();
                var checkbank = await _context.AdminBankAccounts
                     .Where(c => c.Iban.ToLower() == bank.Iban.ToLower()
                       && c.AccountNumber.ToLower() == c.AccountNumber.ToLower())
                     .AnyAsync();
                if (checkbank)
                {
                    _toastNotification.AddErrorToastMessage(lang == "ar" ? "رقم الايبان او الحساب موجود من قبل" : "The IBAN Or account number already exist", new ToastrOptions { Title = "" });
                    return View(bank);
                }
                bank.IsActive = true;
                _context.Add(bank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bank);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _context.AdminBankAccounts.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }
            return View(bank);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, AdminBankAccount bank)
        {
            if (id != bank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var lang = Helper.GetLanguage();
                try
                {
                    var oldBank = await _context.AdminBankAccounts
                                  .AsNoTracking()
                                 .FirstOrDefaultAsync(c => c.Id == id);
                    if (oldBank.Iban.ToLower() != bank.Iban.ToLower() || oldBank.AccountNumber.ToLower() != bank.AccountNumber.ToLower())
                    {
                        var checkbank = await _context.AdminBankAccounts
                             .Where(c => c.Iban.ToLower() == bank.Iban.ToLower()
                                    || c.AccountNumber.ToLower() == c.AccountNumber.ToLower())
                                .AnyAsync();

                        if (checkbank)
                        {
                            _toastNotification.AddErrorToastMessage(lang == "ar" ? "رقم الايبان او الحساب موجود من قبل" : "The IBAN Or  account number already exist", new ToastrOptions { Title = "" });
                            return View(bank);
                        }

                    }

                    _context.AdminBankAccounts.Update(bank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bank);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            var bank = await _context.AdminBankAccounts.FindAsync(id);

            bank.IsActive = !bank.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { key = 1, data = bank.IsActive });
        }
    }
}
