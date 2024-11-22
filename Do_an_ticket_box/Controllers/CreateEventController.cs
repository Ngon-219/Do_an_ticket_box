using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Do_an_ticket_box.Controllers
{
    public class CreateEventController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly ApplicationDbContext _context;

        public CreateEventController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = dbContext;
        }
        public IActionResult EventCreated()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTicket(IFormFile? event_img, string event_name, string event_location, string event_note, string[] ticketType, decimal[] Price, int[] SeatNumber, DateTime[] StartTime, DateTime StartTimeEvent, DateTime EndTimeEvent, string description)
        {
            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(event_img.FileName);
                string userPath = Path.Combine(wwwRootPath, "Images", "Events");

                if (!Directory.Exists(userPath))
                {
                    Directory.CreateDirectory(userPath);
                }

                string fullPath = Path.Combine(userPath, fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await event_img.CopyToAsync(fileStream);
                }

                var userEmail = Request.Cookies["UserEmail"];
                var userId = await this._context.User.FirstOrDefaultAsync(e => e.Email == userEmail);

                var newEvent = new Event
                {
                    UserID = userId.UserID,
                    Event_Name = event_name,
                    Event_date = StartTimeEvent,
                    Event_date_end = EndTimeEvent,
                    Event_time = TimeOnly.FromDateTime(StartTimeEvent),
                    Event_time_end = TimeOnly.FromDateTime(EndTimeEvent),
                    location = event_location + " " + event_note,
                    description = description,
                    created_at_time = DateTime.Now,
                    event_image = $"/Images/Events/{fileName}",
                    countClick = 0,
                };

                await this._context.Events.AddAsync(newEvent);
                await this._context.SaveChangesAsync();

                var eventId = await this._context.Events.FirstOrDefaultAsync(e => e.Event_time == newEvent.Event_time && e.Event_Name == newEvent.Event_Name);

                for (var i = 0; i < ticketType.Length; i++)
                {
                    var newTicket = new Ticket
                    {
                        Event_ID = eventId.Event_ID,
                        Ticket_type = ticketType[i],
                        price = Price[i],
                        seat_number = SeatNumber[i],
                        status = "remain",
                        seat_remain = SeatNumber[i],
                        start_time = StartTime[i],
                    };
                    await this._context.Ticket.AddAsync(newTicket);
                    await this._context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi upload ảnh: {ex.Message}";
                Console.WriteLine(ex);
                return NotFound();
            }
            return Content(description + " " + event_name + " " + event_location + " " + event_note + ticketType[1] + " " + Price[1].ToString() + " " + SeatNumber[1].ToString() + " " + StartTime[0].ToString());
        }
    }
}
