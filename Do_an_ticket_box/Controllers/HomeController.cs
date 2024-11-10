using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

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
        public IActionResult Index(int? id)
        {
            var events = this._context.Events.ToList();

            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            var events_in_month = this._context.Set<Event>()
                .Where(e => (e.Event_date_end.Month == currentMonth || e.Event_date.Month == currentMonth))
                .Take(8)
                .ToList();
            ViewData["events_in_month"] = events_in_month;
            return View(events);
        }

        public IActionResult Error404()
        {
            return View();
        }

        public IActionResult EventInMonth(int page) {
            /* int pageIndex = 1; // Trang đầu tiên
             int pageSize = 9;

             var paginatedItems = _context.Events
                 .Skip((pageIndex - 1) * pageSize)
                 .Take(pageSize)
                 .ToList();
 */

            int pageIndex = page;
            
            return Content("ahihi");
        }

        
        public IActionResult SearchResult()
        {
            return View();
        }
/*
        [Route("/search")]*/
        public IActionResult noSearchResult() { 
            return View();
        }
        public async Task<IActionResult> Ticket (int id)
        {
            var EventInfor = await this._context.Events.FindAsync(id);
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
