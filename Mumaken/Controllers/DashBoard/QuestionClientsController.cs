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
    [AuthorizeRoles(Roles.Admin, Roles.CommonQuestion)]
    public class QuestionClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public QuestionClientsController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CommonQuestions.OrderByDescending(c => c.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CommonQuestion questions)
        {
            if (ModelState.IsValid)
            {
                questions.IsActive = true;
                _context.Add(questions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            _toastNotification.AddSuccessToastMessage(Helper.GetLanguage() == "ar" ? "تم الاضافة بنجاح" : "Added successfully", new ToastrOptions { Title = "" });
            return View(questions);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.CommonQuestions.FindAsync(id);
            if (questions == null)
            {
                return NotFound();
            }
            return View(questions);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CommonQuestion questions)
        {
            if (id != questions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsExists(questions.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _toastNotification.AddSuccessToastMessage(Helper.GetLanguage() == "ar" ? "تم التعديل بنجاح" : "Modified successfully", new ToastrOptions { Title = "" });
                return RedirectToAction(nameof(Index));
            }
            return View(questions);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeState(int? id)
        {
            var question = await _context.CommonQuestions.FindAsync(id);

            question.IsActive = !question.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { key = 1, data = question.IsActive });
        }
        private bool QuestionsExists(int id)
        {
            return _context.CommonQuestions.Any(e => e.Id == id);
        }
    }
}
