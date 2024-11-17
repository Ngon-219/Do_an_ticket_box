using Do_an_ticket_box.Services;
using Do_an_ticket_box.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Do_an_ticket_box.Controllers
{
    public class MyTicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyTicketController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public IActionResult Index(int? id, string status = "All", string? filter = null)
        {
           
            filter ??= "Upcoming";
            var ticketQuery = from Booking in _context.Bookings
                              join Event in _context.Events on Booking.Event_ID equals Event.Event_ID
                              join Ticket in _context.Ticket on Booking.Ticket_ID equals Ticket.Ticket_ID
                              //where Booking.User_ID == id
                              select new MyTicketVM
                              {
                                  EventName = Event.Event_Name,
                                  status = Booking.status,
                                  Ordercode = Booking.Booking_ID,
                                  date = Event.Event_date,
                                  timeStart = Event.Event_time,
                                  timeEnd = Event.Event_time_end,
                                  location = Event.location
                              };

            if (status == "Success")
            {
                ticketQuery = ticketQuery.Where(t => t.status == "Thành Công");
            }
            else if (status == "Processing")
            {
                ticketQuery = ticketQuery.Where(t => t.status == "Đang Xử Lí");
            }

            if (filter == "Upcoming")
            {
                ticketQuery = ticketQuery.Where(t => t.date >= DateTime.Now);
            }
            else if (filter == "Completed")
            {
                ticketQuery = ticketQuery.Where(t => t.date < DateTime.Now);
            }

            var tickets = ticketQuery.ToList();
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentFilter = filter;

            return View(tickets);
        }
        
    }
}
