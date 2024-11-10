using Microsoft.AspNetCore.Mvc;

namespace Do_an_ticket_box.Controllers
{
    public class CreateEventController : Controller
    {
        public IActionResult EventCreated()
        {
            return View();
        }
    }
}
