using Do_an_ticket_box.Areas.Manager.Models;
using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Do_an_ticket_box.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_an_ticket_box.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class BannerController : Controller
    {

        private readonly ApplicationDbContext _context;

        public BannerController(ApplicationDbContext context)
        {
            this._context = context;
        }


        public async Task<IActionResult> Index()
        {
            var userManage = Request.Cookies["UserEmailManage"];
            if (userManage != null)
            {
                /*                var user = await this._context.User.ToListAsync();
                                ViewData["userData"] = user;
                                var countUser = await this._context.User.CountAsync();
                                ViewData["totalPage"] = (int)Math.Ceiling((double)countUser / 1);*/

                return View();
            }
            return Content("user");
        }
    }
}
