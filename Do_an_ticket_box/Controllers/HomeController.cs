using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Do_an_ticket_box.Controllers
{
/*    [Route("[controller]/[action]")]*/
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
        public IActionResult Ticket ()
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
