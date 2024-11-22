using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_an_ticket_box.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EventController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index(string location, int page)
        {
            ViewData["location"] = location;
            int pageIndex = page;
            string searchPattern = $"%{location}%"; // Chuỗi cho LIKE
            var totalPage = await this._context.Events
                .Where(e => EF.Functions.Like(e.location, searchPattern))
                .CountAsync();
            totalPage = totalPage / 10 + 1;
            ViewBag.currentPage = pageIndex;
            ViewBag.TotalPage = (int)totalPage;
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            if (pageIndex <= totalPage)
            {
                var paginatedEvent = await this._context.Set<Event>()
                    .Where(e => EF.Functions.Like(e.location, searchPattern))
                    .Skip((pageIndex - 1) * 10)
                    .Take(10)
                    .ToListAsync();
                return View(paginatedEvent);
            }
            return NotFound();
            
        }
    }
}
