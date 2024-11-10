using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Do_an_ticket_box.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using NuGet.Versioning;
using Do_an_ticket_box.ViewModels;

namespace Do_an_ticket_box.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        
        public async Task<IActionResult> Index(int id)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.UserID == id);
            if (user != null) {
                var viewModel = new UpdateUserViewModel()
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    UserSurname = user.UserSurname,
                    avatarImg = user.avatarImg,
                    Email = user.Email,
                    Phone = user.Phone,
                    gender = user.gender,
                    birthday = user.birthday

                };
                return await Task.Run(()=>View("Index",viewModel));
            }
            return RedirectToAction("Index");
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UpdateUserViewModel model, IFormFile avatarImgFile)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("Index", model);
            //}

            var user = await _dbContext.User.FindAsync(model.UserID);
            if (user != null)
            {
                if (avatarImgFile != null && avatarImgFile.Length > 0)
                {

                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + avatarImgFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarImgFile.CopyToAsync(fileStream);
                    }

                    user.avatarImg = uniqueFileName;
                }

                user.UserName = model.UserName;
                user.UserSurname = model.UserSurname;
                user.Phone = model.Phone;
                user.gender = model.gender;
                user.birthday = model.birthday;

                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index", new { id = user.UserID });
            }
            return RedirectToAction("Index");
        }

    }
}
