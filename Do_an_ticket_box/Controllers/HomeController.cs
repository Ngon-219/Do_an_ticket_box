using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Security.Principal;

namespace Do_an_ticket_box.Controllers
{
/*    [Route("[controller]/[action]")]*/
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _connectionString = configuration.GetConnectionString("defaultString");
        }

/*        [Route("/")]*/
        public async Task<IActionResult> Index(int? id)
        {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            var events = await this._context.Events.Where(e => e.status != "unvertify" && (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth))
            .OrderByDescending(e => e.countClick)
            .Take(10) 
            .ToListAsync();




            var count_event_in_month = await this._context.Set<Event>()
                .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth) && e.status != "unvertify")
                .CountAsync();
            var events_in_month = await this._context.Set<Event>()
                .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth) && e.status != "unvertify")
                .Take(8)
                .OrderByDescending(e => e.Event_date)
                .ToListAsync();
            
            var eventInHaNoi =await this._context.Events
                .Where(e => EF.Functions.Like(e.location, "%Hà Nội%") && e.status != "unvertify")
                .Take(8)
                .OrderByDescending(e => e.Event_date)
                .ToListAsync();
            var count_event_in_HaNoi = await this._context.Events
                .Where(e => EF.Functions.Like(e.location, "%Hà Nội%") && e.status != "unvertify")
                .CountAsync();
            var count_events_in_month = await this._context.Set<Event>()
            .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth) && e.status != "unvertify")
            .CountAsync();

            var userEmail = Request.Cookies["UserEmail"];
            var user = await this._context.User.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user != null)
            {
                if(user.status == "lock")
                {
                    return RedirectToAction("Logout", "Account");
                } else
                {
                    ViewData["userStatus"] = user.status;
                    Console.WriteLine(ViewData["userStatus"]);
                }
            }
            else ViewData["userStatus"] = "unlogin";

            ViewData["count_event_in_HaNoi"] = count_event_in_HaNoi;
            ViewData["events_in_HaNoi"] = eventInHaNoi;
            ViewData["events_in_month"] = events_in_month;
            ViewData["count_event_in_month"] = count_event_in_month;
            return View(events);
        }

        public IActionResult Error404()
        {
            return View();
        }

        public async Task<IActionResult> EventInMonth(int page) {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            int pageIndex = page;
            var totalPage = await this._context.Events.Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth) && e.status != "unvertify").CountAsync();
            totalPage = (int)Math.Ceiling(totalPage / 10.0);
            ViewBag.currentPage = pageIndex;
            ViewBag.TotalPage = (int)totalPage;

            if (pageIndex <= totalPage)
            {
                var paginatedEvent = await this._context.Set<Event>()
                    .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth) && e.status != "unvertify")
                    .Skip((pageIndex - 1) * 10)
                    .OrderByDescending(e => e.Event_date)
                    .Take(10)
                    .ToListAsync();

            return View(paginatedEvent);
            } return NotFound();
        }

        
        //public IActionResult SearchResult()
        //{
        //    return View();
        //}
/*
        [Route("/search")]*/
        //public IActionResult noSearchResult() { 
        //    return View();
        //}
        public async Task<IActionResult> Ticket (int id)
        {
            var EventInfor = await this._context.Events
                .Where(e => e.status != "unvertify" && e.Event_ID == id)
                .FirstOrDefaultAsync();

            if (EventInfor == null)
            {
                return NotFound();
            }

            if (EventInfor != null) { 
                EventInfor.countClick += 1;
            }
            await this._context.SaveChangesAsync();
            var ticket = this._context.Ticket
            .Include(t => t.Event)
                .Where(t => t.Event_ID == id && t.status != "unvertify")
                .ToList();
            ViewData["ticket"] = ticket;

            var minPriceForEvent = this._context.Ticket
               .Where(t => t.Event_ID == id)
               .OrderBy(t => t.price)
               .Take(1)                .Union(
                   this._context.Ticket
                       .Where(t => t.Event_ID == id)
                       .OrderBy(t => t.price)
                       .Take(1)
               )
               .FirstOrDefault();
            ViewData["min_price"] = minPriceForEvent.price;
            ViewData["check_seat"] = minPriceForEvent.seat_remain;

            return View(EventInfor);
        }

        public async Task<IActionResult> AllEvent(int page, string? filter, string? sorting)
        {
            int pageIndex = page;
            var totalPage = await this._context.Events.Where(e => e.status != "unvertify").CountAsync();
            totalPage = (int)Math.Ceiling(totalPage / 10.0);
            ViewBag.currentPage = pageIndex;
            ViewBag.TotalPage = (int)totalPage;
            Console.WriteLine("Ở trang all event " + filter + " " + sorting + " " + totalPage );

            var query = this._context.Events.AsQueryable().Where(e => e.status != "unvertify");

            if (!string.IsNullOrEmpty(filter) && filter != "null")
            {
                query = query.Where(e => e.location.Contains(filter));
            }

            if (!string.IsNullOrEmpty(sorting) && sorting != "null")
            {
                query = sorting switch
                {
                    "asc" => query.OrderBy(e => e.Event_date),
                    "desc" => query.OrderByDescending(e => e.Event_date),
                    _ => query.OrderBy(e => e.Event_date)
                };
            }


            if (pageIndex <= totalPage)
            {
                var paginatedEvent = await query
                    .Skip((pageIndex - 1) * 10)
                    .Take(10)
                    .ToListAsync();

                if (paginatedEvent.Count > 0)
                {
                    return View(paginatedEvent);

                }
                else
                {
                    return RedirectToAction("EventNull", "Home");
                }

            }
            return NotFound();
        }

        public IActionResult EventNull()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
