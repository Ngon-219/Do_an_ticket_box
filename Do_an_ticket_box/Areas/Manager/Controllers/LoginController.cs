using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace Do_an_ticket_box.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Home()
        {
            var userManage = Request.Cookies["UserEmailManage"];
            if (userManage != null)
            {
                var countUser = await this._context.User.CountAsync();
                ViewData["usersCount"] = countUser;

                var countEvent = await this._context.Events.CountAsync();
                ViewData["countEvent"] = countEvent;

                var countReport = await this._context.Reports.CountAsync();
                ViewData["countReport"] = countReport;

                var total = await this._context.Ticket
                .SumAsync(t => (((t.seat_number ?? 0) - (t.seat_remain ?? 0)) * (t.price ?? 0)));

                ViewData["revenue"] = total;


                return View();
            }
            return View("Index");
        }

        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            Response.Cookies.Delete("UserEmailManage");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> Login(string userEmail, string password)
        {
            var user = this._context.User.FirstOrDefault(u => u.Email == userEmail);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password) && user.role == "admin")
            {
                var claims = new List<Claim>
                {
                    new Claim("UserEmailManage", userEmail),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity),
                   new AuthenticationProperties
                   {
                       IsPersistent = true,
                       ExpiresUtc = DateTime.UtcNow.AddDays(7)
                   });


                Response.Cookies.Append("UserEmailManage", user.Email, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });
                return Json(new { success = true, message = "Đăng nhập thành công!" });
            }

            return Json(new { success = false, message = "Tên đăng nhập hoặc mật khẩu sai." });


        }

        public ActionResult GetDetailsByMonth(int? year, string type)
        {
            /*       var monthlyRevenue = _context.Payments
                      .Join(_context.Bookings,
                            payment => payment.Booking_ID,
                            booking => booking.Booking_ID,
                            (payment, booking) => new { payment, booking })
                      .Join(_context.Events,
                            combined => combined.booking.Event_ID,
                            ev => ev.Event_ID,
                            (combined, ev) => new { combined.payment, combined.booking, ev })
                      .Where(x => x.payment.Payment_time.Year == year) // Lọc theo năm custom
                      .GroupBy(x => x.payment.Payment_time.Month)      // Nhóm theo tháng
                      .Select(g => new
                      {
                          Month = g.Key,
                          TotalRevenue = g.Sum(x => x.payment.Amount_paid)
                      })
                      .OrderBy(result => result.Month)
                      .ToList();*/

            var months = Enumerable.Range(1, 12);

            switch (type)
            {
                case "Users":
                    {
                        var result = (from month in months
                                      join monthlyData in
                                          (from user in this._context.User
                                           join token in this._context.EmailVerificationTokens
                                               on user.EmailVerificationTokenId equals token.Id
                                           where token.CreateOnUtc.Year == year
                                           group user by token.CreateOnUtc.Month into monthlyGroup
                                           select new
                                           {
                                               Month = monthlyGroup.Key,
                                               UserCount = monthlyGroup.Count()
                                           })
                                      on month equals monthlyData.Month into monthlyJoin
                                      from joinedData in monthlyJoin.DefaultIfEmpty()
                                      select new
                                      {
                                          UserCount = joinedData != null ? joinedData.UserCount : 0
                                      })
                          .Select(x => x.UserCount) 
                          .ToArray(); 

                        return Json(new { success = true, data = result });
                    }
                case "Events":
                    {
                        var result = (from month in months
                                      join monthlyData in
                                          (from e in this._context.Events
                                           where e.Event_date.Year == year
                                           group e by e.Event_date.Month into monthlyGroup
                                           select new
                                           {
                                               Month = monthlyGroup.Key,
                                               UserCount = monthlyGroup.Count()
                                           })
                                      on month equals monthlyData.Month into monthlyJoin
                                      from joinedData in monthlyJoin.DefaultIfEmpty()
                                      select new
                                      {
                                          UserCount = joinedData != null ? joinedData.UserCount : 0
                                      })
                          .Select(x => x.UserCount)
                          .ToArray();
                        return Json(new { success = true, data = result });
                    }
                case "Reports":
                    {
                        var result = (from month in months
                                      join monthlyData in
                                          (from r in this._context.Reports
                                           join e in this._context.Events
                                               on r.Event_ID equals e.Event_ID
                                           where e.Event_date.Year == year
                                           group r by e.Event_date.Month into monthlyGroup
                                           select new
                                           {
                                               Month = monthlyGroup.Key,
                                               UserCount = monthlyGroup.Count()
                                           })
                                      on month equals monthlyData.Month into monthlyJoin
                                      from joinedData in monthlyJoin.DefaultIfEmpty()
                                      select new
                                      {
                                          UserCount = joinedData != null ? joinedData.UserCount : 0
                                      })
                          .Select(x => x.UserCount)
                          .ToArray();
                        return Json(new { success = true, data = result });
                    }
                case "Revenue":
                    {
                        var result = (from month in months
                                      join monthlyData in
                                          (from t in this._context.Ticket
                                           where t.start_time.Year == year
                                           group t by t.start_time.Month into monthlyGroup
                                           select new
                                           {
                                               Month = monthlyGroup.Key,
                                               UserCount = monthlyGroup.Sum(t => (((t.seat_number ?? 0) - (t.seat_remain ?? 0)) * (t.price ?? 0)))
                                           })
                                      on month equals monthlyData.Month into monthlyJoin
                                      from joinedData in monthlyJoin.DefaultIfEmpty()
                                      select new
                                      {
                                          UserCount = joinedData != null ? joinedData.UserCount : 0
                                      })
                          .Select(x => x.UserCount)
                          .ToArray();
                        return Json(new { success = true, data = result });
                    }
                default:
                    {
                        return Json(new { success = false });
                    }

            }

            
        }
    }
}
