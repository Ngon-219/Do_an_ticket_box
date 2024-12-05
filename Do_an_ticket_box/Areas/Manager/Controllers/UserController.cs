using Do_an_ticket_box.Areas.Manager.Models;
using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Do_an_ticket_box.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_an_ticket_box.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class UserController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
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

        public async Task<IActionResult> pagnination(int page = 1, int pageSize = 5)
        {
            Console.WriteLine(page + " " +  pageSize);  
            var userManage = Request.Cookies["UserEmailManage"];
            if (userManage != null)
            {
                var totalUsers = await _context.User.CountAsync();
                var users = await _context.User
                    .OrderBy(u => u.UserID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Json(new
                {
                    success = true,
                    userdata = users,
                    totalUsersdata = totalUsers,
                    currentPage = page,
                    totalPages = (int)Math.Ceiling((double)totalUsers / pageSize)
                });
            }
            return Json(new { success = false, message = "Unauthorized" });
        }


        [HttpPost]
/*        [ValidateAntiForgeryToken]*/
        public async Task<JsonResult> UpdateStatus([FromBody] Do_an_ticket_box.Areas.Manager.Models.updateStatus req)
        {
            var userManage = Request.Cookies["UserEmailManage"];
            if (req == null || userManage == null)
            {
                return Json(new { success = false, message = "update không thành công" });
            }
            Console.WriteLine(req.UserID + " " + req.status);
            var updateUser = await this._context.User.FirstOrDefaultAsync(e => e.UserID == req.UserID);

            if (updateUser != null)
            {
                updateUser.status = req.status;
                await this._context.SaveChangesAsync();
                Console.WriteLine("ahihi");
                return Json(new { success = true, message = "update thành công" });
            }

            return Json(new { success = false, message = "update không thành công" });
        }

        [HttpPost]
        public JsonResult addManager()
        {
            return Json(new { success = false });
        }

        public IActionResult addManagerView()
        {
            return View();
        }
        [HttpPost]        
        
        public async Task<IActionResult> Register([FromBody] registerUserDtos registerUser)
        {
            Console.WriteLine(registerUser);
            if (ModelState.IsValid)
            {
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == registerUser.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return Json(new { success = false, message = "email đã được sử dụng" });
                }

                // Hash mật khẩu
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

                // Tạo đối tượng người dùng mới
                var newUser = new User
                {
                    Email = registerUser.Email,
                    Password = hashedPassword,
                    avatarImg = "https://media.istockphoto.com/id/1341046662/vector/picture-profile-icon-human-or-people-sign-and-symbol-for-template-design.jpg?s=612x612&w=0&k=20&c=A7z3OK0fElK3tFntKObma-3a7PyO8_2xxW0jtmjzT78=",
                    status = "admin",
                    role = "admin",
                    // Gán các thuộc tính khác nếu cần
                };


                // Thêm người dùng vào database
                _context.User.Add(newUser);
                await _context.SaveChangesAsync();

                // Chuyển hướng tới trang đăng nhập
                return Json(new { success = true, message = "Thành công" });
            }
            return Json(new { success = false, message = "Lỗi" });
        }

        [HttpPost]
        [HttpPost]
        public JsonResult Search([FromBody] searchReq search)
        {
            Console.WriteLine($"Search term: {search.Search}");

            var users = this._context.User
                         .Where(e => e.Email.ToLower().Contains(search.Search.ToLower()))
                         .ToList();

            return Json(new { success = true, userdata = users, totalPages = 0 });
        }

    }
}
