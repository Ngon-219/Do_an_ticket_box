using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;

namespace Do_an_ticket_box.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly ApplicationDbContext _context;

        public EmailController(IFluentEmail fluentEmail, ApplicationDbContext context)
        {
            _fluentEmail = fluentEmail;
            _context = context;
        }
        [HttpGet("vertify-email")]
        public ActionResult vertifyEmail()
        {
            var userEmail = Request.Cookies["UserEmail"];
            var user = this._context.User.FirstOrDefault(x => x.Email == userEmail);
            if (user == null) { return RedirectToAction("Login", "Account"); }
            if (user.status == "vertify") return View();
            var emailToken = this._context.EmailVerificationTokens
                .Where(x => x.UserId == user.UserID)
                .OrderByDescending(x => x.ExpiresOnUtc)
                .FirstOrDefault();

            if (emailToken.ExpiresOnUtc > DateTime.UtcNow)
            {
                Console.WriteLine("first " + emailToken.ExpiresOnUtc.ToString());
                Console.WriteLine("second " + DateTime.UtcNow.ToString());

                user.status = "vertify";
                this._context.SaveChanges();
                return RedirectToAction("VertifySucc");
            }
            return RedirectToAction("TokenExpries");

        }
        [HttpGet("token-expries")]
        public ActionResult TokenExpries()
        {
            return View();
        }
        [HttpGet("vertify-success")]  
        
        public ActionResult VertifySucc()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var userEmail = Request.Cookies["UserEmail"];
            Console.WriteLine(userEmail);
            if (userEmail == null)
            {
                return NotFound();
            }

            var user = this._context.User.FirstOrDefault(x => x.Email == userEmail);

            if (user.status == "unvertify")
            {
                var verificationToken = new EmailVerificationToken
                {
                    Id = new Guid(),
                    UserId = user.UserID,
                    CreateOnUtc = DateTime.UtcNow,
                    ExpiresOnUtc = DateTime.UtcNow.AddMinutes(10)
                };
                this._context.EmailVerificationTokens.Add(verificationToken);
                await this._context.SaveChangesAsync();

                var verificationLink = Url.Action(
                    "vertifyEmail",
                    "Email",
                    null,
                    protocol: Request.Scheme,
                    host: "localhost:7132"
                );
                await _fluentEmail
                    .To(userEmail)
                    .Subject("Email Verification for TicketBox")
                    .Body($"To verify your email address, <a href='{verificationLink}'>Click here</a>", isHtml: true)
                    .SendAsync();
            }
            return View();
        }
    }
}
