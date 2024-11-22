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
            var events = await this._context.Events
            .OrderByDescending(e => e.countClick) 
            .Take(10) 
            .ToListAsync();


            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            var count_event_in_month = await this._context.Set<Event>()
                .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth))
                .CountAsync();
            var events_in_month = await this._context.Set<Event>()
                .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth))
                .Take(8)
                .ToListAsync();
            // sự kiện tại hà nội
            var eventInHaNoi =await this._context.Events
                .Where(e => EF.Functions.Like(e.location, "%Hà Nội%"))
                .Take(8)
                .ToListAsync();
            var count_event_in_HaNoi = await this._context.Events
                .Where(e => EF.Functions.Like(e.location, "%Hà Nội%"))
                .CountAsync();
            var count_events_in_month = await this._context.Set<Event>()
            .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth))
            .CountAsync();

            var userEmail = Request.Cookies["UserEmail"];
            var user = await this._context.User.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user != null)
            {
                ViewData["userStatus"] = user.status;
                Console.WriteLine(ViewData["userStatus"]);
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
            var totalPage = await this._context.Events.Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth)).CountAsync();
            totalPage = totalPage / 10 + 1;
            ViewBag.currentPage = pageIndex;
            ViewBag.TotalPage = (int)totalPage;

            if (pageIndex <= totalPage)
            {
                var paginatedEvent = await this._context.Set<Event>()
                    .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth))
                    .Skip((pageIndex - 1) * 10)
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
            var EventInfor = await this._context.Events.FindAsync(id);
            if (EventInfor != null) { 
                EventInfor.countClick += 1;
            }
            await this._context.SaveChangesAsync();
            var ticket = this._context.Ticket
            .Include(t => t.Event)
                .Where(t => t.Event_ID == id)
                .ToList();
            ViewData["ticket"] = ticket;

            var minPriceForEvent = this._context.Ticket
                .Where(t => t.Event_ID == id)
                .Min(t => t.price);
            ViewData["min_price"] = minPriceForEvent;

            return View(EventInfor);
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
