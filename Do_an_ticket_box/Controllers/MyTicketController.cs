using Do_an_ticket_box.Models;
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
            var userEmail = Request.Cookies["UserEmail"];
            Console.WriteLine("useremai la: " + userEmail);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var user = this._context.User.FirstOrDefault(x => x.Email == userEmail);
                Console.WriteLine(user.UserName);
                @ViewData["user"] = user.UserSurname + " " + user.UserName;
                @ViewData["userAvt"] = user.avatarImg;
                var result = from Booking in this._context.Bookings
                             join Event in this._context.Events on Booking.Event_ID equals Event.Event_ID
                             join Ticket in this._context.Ticket on Booking.Ticket_ID equals Ticket.Ticket_ID
                             where Booking.User_ID == user.UserID && Booking.status == "COMPLETED"
                             select new MyTicketVM
                             {
                                 EventName = Event.Event_Name,
                                 status = Booking.status,
                                 Ordercode = Booking.Booking_ID,
                                 date = Event.Event_date,
                                 timeStart = Event.Event_time,
                                 timeEnd = Event.Event_time_end,
                                 location = Event.location,
                                 ticket_type = Ticket.Ticket_type,
                                 Quanlity = Booking.Quanlity,
                             };
                if (filter == "Upcoming")
                {
                    result = result.Where(t => t.date >= DateTime.Now);
                }
                else if (filter == "Completed")
                {
                    result = result.Where(t => t.date < DateTime.Now);
                }

                var tickets = result.ToList();
                ViewBag.CurrentStatus = status;
                ViewBag.CurrentFilter = filter;
                return View(tickets);
            }
        }
        
    }
}
