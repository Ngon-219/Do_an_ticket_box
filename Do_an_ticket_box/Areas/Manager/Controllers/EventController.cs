using Do_an_ticket_box.Areas.Manager.Models;
using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Do_an_ticket_box.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_an_ticket_box.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class EventController : Controller
    {

        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> pagnination(int page = 1, int pageSize = 5, string filter = "")
        {
            Console.WriteLine(page + " " + pageSize);
            var userManage = Request.Cookies["UserEmailManage"];
            if (userManage != null)
            {
                var totalEvents = 0;
                IQueryable<Event> eventsQuery = _context.Events;  

                if (!string.IsNullOrEmpty(filter))
                {
                    eventsQuery = eventsQuery.Where(e => e.location.ToLower().Contains(filter.ToLower()));
                }

                totalEvents = await eventsQuery.CountAsync();

                var events = await eventsQuery
                    .OrderByDescending(u => u.Event_date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(); 

                return Json(new
                {
                    success = true,
                    eventData = events,
                    totalEventData = totalEvents,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling((double)totalEvents / pageSize)
                });
            }

            return Json(new { success = false, message = "Unauthorized" });
        }

        public async Task<IActionResult> pagninationBanner(int page = 1, int pageSize = 5, string filter = "")
        {
            Console.WriteLine(page + " " + pageSize);
            var userManage = Request.Cookies["UserEmailManage"];
            if (userManage != null)
            {
                var totalEvents = 0;
                IQueryable<Event> eventsQuery = _context.Events;
                eventsQuery = eventsQuery.Where(e => e.status != "unvertify");

                if (!string.IsNullOrEmpty(filter))
                {
                    eventsQuery = eventsQuery.Where(e => e.location.ToLower().Contains(filter.ToLower()));
                }

                totalEvents = await eventsQuery.CountAsync();

                var events = await eventsQuery
                    .OrderByDescending(u => u.Event_date)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Json(new
                {
                    success = true,
                    eventData = events,
                    totalEventData = totalEvents,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling((double)totalEvents / pageSize)
                });
            }

            return Json(new { success = false, message = "Unauthorized" });
        }



        [HttpPost]
        public JsonResult Search([FromBody] searchReq search)
        {
            Console.WriteLine($"Search term: {search.Search}");

            var users = this._context.Events
                         .Where(e => e.Event_Name.ToLower().Contains(search.Search.ToLower()))
                         .ToList();

            return Json(new { success = true, userdata = users, totalPages = 0 });
        }

        [HttpPost]
        /*        [ValidateAntiForgeryToken]*/
        public async Task<JsonResult> UpdateStatus([FromBody] Do_an_ticket_box.Areas.Manager.Models.updateStatus req)
        {
            var userManage = Request.Cookies["UserEmailManage"];
            if (req == null || userManage == null)
            {
                return Json(new { success = false, message = "update không thành công" });
            }
            Console.WriteLine(req.UserID + " " + req.status);
            var updateEvent = await this._context.Events.FirstOrDefaultAsync(e => e.Event_ID == req.UserID);

            if (updateEvent != null)
            {
                updateEvent.status = req.status;
                await this._context.SaveChangesAsync();
                Console.WriteLine("ahihi");
                return Json(new { success = true, message = "update thành công" });
            }

            return Json(new { success = false, message = "update không thành công" });
        }
    }
}
