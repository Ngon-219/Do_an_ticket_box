using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_an_ticket_box.Controllers
{
    public class MyEventController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MyEventController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userEmail = Request.Cookies["UserEmail"];
            if(userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            } else
            {
                var userId = await this._context.User.FirstOrDefaultAsync(e => e.Email == userEmail);


                var myEvent = await this._context.Set<Event>()
                    .Where(e => e.UserID == userId.UserID && e.status != "unvertify")
                    .ToListAsync();


                return View(myEvent);
            }
        }

        public IActionResult Details(int? id)
        {
            ViewData["eventId"] = id;
            var userEmail = Request.Cookies["UserEmail"];
            var userManage = Request.Cookies["UserEmailManage"];
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            } else
            {
                var ticket = this._context.Set<Ticket>()
                    .Where(e => e.Event_ID == id && e.status != "unvertify")
                    .ToList();
                ViewData["ticket"] = ticket;
                return View();

            }
        }

        public IActionResult DetailsManage(int? id)
        {
            ViewData["eventId"] = id;
            var userManage = Request.Cookies["UserEmailManage"];
            if (userManage == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var ticket = this._context.Set<Ticket>()
                    .Where(e => e.Event_ID == id && e.status != "unvertify")
                    .ToList();
                var result = (from e in this._context.Events
                              join u in this._context.User on e.UserID equals u.UserID
                              where e.Event_ID == id
                              select new
                              {
                                  u.Email
                              }).FirstOrDefault();

                ViewData["userHost"] = result.Email;
                ViewData["ticket"] = ticket;
                return View();

            }
        }
    }
}
