using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Do_an_ticket_box.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Search
        public IActionResult Index(string query)
        {
            var result = this._context.Events
                        .Where(r => (r.Event_Name.ToLower().Contains(query) || r.location.ToLower().Contains(query) || r.description.ToLower().Contains(query)) && r.status != "unvertify")
                        .ToList();
            return View(result);

        }

    }
}
