using Do_an_ticket_box.DTOs;
using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json.Nodes;

namespace Do_an_ticket_box.Controllers
{
    public class CheckoutController : Controller
    {
        private string PaypalClientId { get; set; } = "";
        private string PaypalSecret { get; set; } = "";
        private string PaypalUrl { get; set; } = "";
        private readonly ApplicationDbContext _context;
        public static int EventId { get; set; } = -1;

        public CheckoutController(IConfiguration configuration, ApplicationDbContext context) {
            this.PaypalClientId = configuration["PaypalSettings:ClientId"];
            this.PaypalSecret = configuration["PaypalSettings:Secret"];
            this.PaypalUrl = configuration["PaypalSettings:Url"];
            this._context = context;
        }

        public async Task<IActionResult> Index(int eventId, string? amount)
        {
            ViewBag.PaypalClientId = this.PaypalClientId;
            var orderId = Request.Cookies["orderId"];
            if (orderId == null)
            {
                return Content("hết thời hạn thanh toán");
            }
            var userEmail = Request.Cookies["UserEmail"];
            if (userEmail == null)
            {
                EventId = eventId;
                return RedirectToAction("Login", "Account");
            }

            var bookings = await (from booking in _context.Bookings
                                  join ticket in _context.Ticket on booking.Ticket_ID equals ticket.Ticket_ID
                                  join evnt in _context.Events on booking.Event_ID equals evnt.Event_ID
                                  where booking.OrderId.ToString() == orderId
                                  select new BookingDtos
                                  {  
                                       TicketName = ticket.Ticket_type,
                                       Quanlity = booking.Quanlity,
                                       location = evnt.location,
                                       eventTime = evnt.Event_time,
                                       eventDate = evnt.Event_date,
                                  }).ToListAsync();

            var eventName = this._context.Events.Where(e => e.Event_ID == eventId).FirstOrDefault().Event_Name;
            ViewData["eventName"] = eventName;
            ViewData["amount"] = amount;
            return View(bookings);
        }


        /*        public async Task<string> Token()
                {
                    return await GetPaypalAccessToken();
                }
        */
        static Guid CreateGuidFromString(string input)
        {
            // Hash chuỗi để tạo byte array
            using (var provider = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return new Guid(hashBytes);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(int[] ticketId, int[] eventId, decimal amount, int[] ticketQuanlity)
        {
            DateTime now = DateTime.Now;
            string dateString = now.ToString("yyyyMMddHHmmssfff");
            Guid guid = CreateGuidFromString(dateString);
            Response.Cookies.Append("orderId", guid.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(3),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });
            var userEmail = Request.Cookies["UserEmail"];
            var user = this._context.User.FirstOrDefault(x => x.Email == userEmail);
            List<Booking> data = new List<Booking>();
            if (user != null)
            {
                for (int i = 0; i < ticketId.Length; i++)
                {
                    if (ticketQuanlity[i] != 0)
                    {
                        var bookings = new Booking
                        {
                            User_ID = user.UserID,
                            Event_ID = eventId[i],
                            Ticket_ID = ticketId[i],
                            total_amout = amount / 25000,
                            status = "Unpaid",
                            Quanlity = ticketQuanlity[i],
                            OrderId = guid,
                        };
                        data.Add(bookings);
                        Console.WriteLine("luc submit form nay" + ticketId[i] + " " + eventId[i] + " " + amount);
                    }
                }
                try
                {
                    await this._context.Bookings.AddRangeAsync(data);
                    await this._context.SaveChangesAsync();
                    return RedirectToAction("Index", new { eventId = eventId[0], amount = amount.ToString() });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error when saving bookings: {ex.Message}");
                    return Content("An error occurred while saving your bookings.");
                }
            } else
            {
                EventId = eventId[0];
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Booking(int eventId)
        {
            var userEmail = Request.Cookies["UserEmail"];
            Console.WriteLine(userEmail);
            var user = this._context.User.FirstOrDefault(x => x.Email == userEmail);
            if (user == null) {
                EventId = eventId;
                return RedirectToAction("Login", "Account"); 
            }
            if (user.status == "vertify" && !string.IsNullOrEmpty(userEmail))
            {
                var ticket = this._context.Ticket.Where(x => x.Event_ID == eventId).ToList();
                var eventOfTicket = this._context.Events.FirstOrDefault(e => e.Event_ID == eventId);
                if (eventOfTicket != null)
                {
                    ViewData["eventName"] = eventOfTicket.Event_Name;
                }
                return View(ticket);
            }
            return  RedirectToAction("VertifyEmailNow");
        }


        public ActionResult VertifyEmailNow()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmout = data["amount"].ToString();
            Console.WriteLine(totalAmout);
            if (totalAmout == null)
            {
                return new JsonResult(new { Id = "" });
            }

            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");

            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", totalAmout);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);


            string accessToken = await GetPaypalAccessToken();
            string url = PaypalUrl + "/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

                requestMessage.Content = new StringContent(createOrderRequest.ToString(), null, "application/json");
                
                var httpResponse = await client.SendAsync(requestMessage);
                
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);

                    if (jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                        return new JsonResult(new {Id = paypalOrderId});
                    }
                }
            }


            return new JsonResult(new { Id = "" });
        }

        [HttpPost]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {

            Console.WriteLine("completeOrrder");
            var orderId = data["orderID"]?.ToString();
            Console.WriteLine(orderId);
            if (orderId == null)
            {
                return new JsonResult("error");
            }

            string accessToken = await GetPaypalAccessToken();

            string url = PaypalUrl + "/v2/checkout/orders/" + orderId + "/capture";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("", null, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);

                    if(jsonResponse != null)
                    {
                        string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                        if(paypalOrderStatus == "COMPLETED")
                        {
                            var order = Request.Cookies["orderId"];
                            if (order != null)
                            {
                                var bookingData = this._context.Bookings.Where(b => b.OrderId.ToString() == order).ToList();
                                foreach (var item in bookingData)
                                {
                                    item.status = paypalOrderStatus;
                                    this._context.SaveChanges();
                                }
                                Response.Cookies.Delete("orderId");
                                return new JsonResult("success");
                            } else
                            {
                                return new JsonResult("soldOut");
                            }
                        }
                    }
                }
            }

            return new JsonResult("error");
        }


        [HttpPost]
        public async Task<JsonResult> SaveBooking([FromBody] JsonObject data)
        {
            var userEmail = Request.Cookies["UserEmail"];
            var userId = this._context.User.FirstOrDefault(u => u.Email == userEmail).UserID;
            var bookingObj = new Booking
            {
                User_ID = userId,
                Event_ID = int.Parse(data["ticketId"]?.ToString()),
                Ticket_ID = int.Parse(data["eventId"]?.ToString()),
                total_amout = decimal.Parse(data["amount"]?.ToString()),
                status = "complete",
                booking_time = DateTime.Now,
            };
            this._context.Bookings.Add(bookingObj);
            await this._context.SaveChangesAsync();
            return new JsonResult("ahihi");
        }
        public async Task<string> GetPaypalAccessToken()
        {
            string accessToken = "";

            string url = this.PaypalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient()) 
            {
                string credential64 =
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalSecret));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credential64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null,
                    "application/x-www-form-urlencoded");
                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string strResponse = await httpResponse.Content.ReadAsStringAsync();

                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }

                }
                return accessToken;
            }
        }
    }
}
