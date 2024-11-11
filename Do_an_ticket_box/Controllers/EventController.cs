using Microsoft.AspNetCore.Mvc;

namespace Do_an_ticket_box.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index(string location, int page)
        {
            /*return Content("ahih" + location + page);*/
            return View();
        }
    }
}
