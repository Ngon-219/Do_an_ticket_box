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
            ViewBag.Query = query;

            // Lấy danh sách sự kiện và vé từ cơ sở dữ liệu
            var events = _context.Events.ToList();
            var tickets = _context.Ticket.ToList();

            // Thực hiện tìm kiếm
            var model = (from e in events
                         join t in tickets on e.Event_ID equals t.Event_ID
                         where string.IsNullOrEmpty(query) || e.Event_Name.ToLower().Contains(query.ToLower())
                         select new Do_an_ticket_box.Models.EventTicketViewModel
                         {
                             Event_ID = e.Event_ID,
                             Event_image = e.event_image,
                             Event_Name = e.Event_Name,
                             Price = t.price,
                             Event_Date = e.Event_date
                         }).ToList();

            var allTickets = (from t in tickets
                              join e in events on t.Event_ID equals e.Event_ID
                              select new Do_an_ticket_box.Models.EventTicketViewModel
                              {
                                  Event_ID = e.Event_ID,
                                  Event_image = e.event_image,
                                  Event_Name = e.Event_Name,
                                  Price = t.price,
                                  Event_Date = e.Event_date
                              }).ToList();

            ViewBag.AllTickets = allTickets;
            return View(model);

        }

    }
}
