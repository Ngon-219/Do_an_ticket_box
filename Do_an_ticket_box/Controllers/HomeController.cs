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
            var ngon = this._context.Events.ToList();
            return View(ngon);
        }

        public IActionResult Error404()
        {
            return View();
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
