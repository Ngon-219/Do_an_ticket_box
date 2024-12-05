using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Do_an_ticket_box.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Do_an_ticket_box.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ReportController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            var events = _dbContext.Events
                .Where(e => e.UserID == userId && e.status != "unvertify") 
                .Select(e => new { e.Event_ID, e.Event_Name }) 
                .ToList();

            var eventList = events.Any()
            ? events.Select(e => new SelectListItem
            {
                Value = e.Event_ID.ToString(),
                Text = e.Event_Name
            }).ToList()
            : new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Không có sự kiện nào", Disabled = true }
            };

            // Tạo ViewModel
            var viewModel = new ReportViewModel
            {
                EventList = eventList
            };

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ReportViewModel reportVM)
        {
            // Lấy UserId từ Claims
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            if (ModelState.IsValid)
            {
                var report = new Report
                {
                    User_ID = userId,
                    Event_ID = reportVM.Event_ID == null ? null : reportVM.Event_ID,
                    comment = reportVM.Comment,
                    rate = reportVM.Rate,
                };

                await _dbContext.AddAsync(report);
                await _dbContext.SaveChangesAsync();

                TempData["Success"] = "Tạo báo cáo thành công!";
                return RedirectToAction("Index");
            }

            foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Model error: {modelError.ErrorMessage}");
            }

            TempData["Error"] = "Có lỗi xảy ra. Vui lòng kiểm tra lại!";
            return View(reportVM);
        }



    }
}
