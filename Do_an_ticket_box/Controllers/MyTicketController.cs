using Microsoft.AspNetCore.Mvc;

namespace Do_an_ticket_box.Controllers
{
    public class MyTicketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
