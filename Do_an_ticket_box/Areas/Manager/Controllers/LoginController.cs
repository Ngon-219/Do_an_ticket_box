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
                .Where(t => t.status == "remain")
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

        public async Task<ActionResult> GetDetailsByMonth()
        {

            return Json(new { success = true });
        }
    }
}
