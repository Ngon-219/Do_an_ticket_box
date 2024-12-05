using Do_an_ticket_box.Areas.Manager.Models;
using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Do_an_ticket_box.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_an_ticket_box.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ReportController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            this._context = context;
        }


        public async Task<IActionResult> Index()
        {
            var userManage = Request.Cookies["UserEmailManage"];
            if (userManage != null)
            {
                return View();
            }
            return Content("user");
        }

        public async Task<IActionResult> pagnination(int page = 1, int pageSize = 5)
        {
            Console.WriteLine(page + " " + pageSize);
            var userManage = Request.Cookies["UserEmailManage"];
            if (userManage != null)
            {
                var totalReports = await _context.Reports.CountAsync();
                /*
                                var result = from user in _context.Users
                                             join order in _context.Orders
                                             on user.UserId equals order.UserId
                                             join product in _context.Products
                                             on order.ProductId equals product.ProductId
                                             select new
                                             {
                                                 UserName = user.Name,
                                                 OrderId = order.OrderId,
                                                 ProductName = product.Name,
                                                 Quantity = order.Quantity
                                             };*/

                var reports = await (from report in this._context.Reports
                              join user in this._context.User
                              on report.User_ID equals user.UserID
                              join events in this._context.Events
                              on report.Event_ID equals events.Event_ID
                              select new
                              {
                                  report_id = report.Report_ID,
                                  useremail = user.Email,
                                  eventName = events.Event_Name,
                                  report = report.comment,
                                  reportRate = report.rate,
                                  eventStart = events.Event_date
                              })
                              .OrderBy(r => r.reportRate)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();


                return Json(new
                {
                    success = true,
                    reportData = reports,
                    totalReport = totalReports,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling((double)totalReports / pageSize)
                });
            }
            return Json(new { success = false, message = "Unauthorized" });
        }


        [HttpPost]
        public JsonResult Search([FromBody] searchReq search)
        {
            Console.WriteLine($"Search term: {search.Search}");

/*            var users = this._context.Events
                         .Where(e => e.Event_Name.ToLower().Contains(search.Search.ToLower()))
                         .ToList();*/

            var reports = (from report in this._context.Reports
                                join user in this._context.User
                                on report.User_ID equals user.UserID
                                join events in this._context.Events
                                on report.Event_ID equals events.Event_ID
                                where events.Event_Name.Contains(search.Search)
                                select new
                                {
                                    report_id = report.Report_ID,
                                    useremail = user.Email,
                                    eventName = events.Event_Name,
                                    report = report.comment,
                                    reportRate = report.rate,
                                    eventStart = events.Event_date
                                })
                                .ToList();

            return Json(new { success = true, reportData = reports, totalPages = 0 });
        }

    }
}
