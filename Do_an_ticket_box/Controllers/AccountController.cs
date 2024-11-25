using Do_an_ticket_box.Models;
using Do_an_ticket_box.Services;
using Do_an_ticket_box.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Do_an_ticket_box.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using NuGet.Versioning;
using Do_an_ticket_box.ViewModels;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using dotenv.net;

namespace Do_an_ticket_box.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _context = dbContext;
        }

        [HttpGet]
        
        public async Task<IActionResult> Index(int ?id)
        {
            var userEmail = Request.Cookies["UserEmail"];
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Email == userEmail);
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
                await _dbContext.SaveChangesAsync();
                return await Task.Run(()=>View("Index",viewModel));
            }
            return RedirectToAction("Index");
            

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UpdateUserViewModel model, IFormFile? avatarImgFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Thông tin không hợp lệ.";
                return View(model);
            }

            var user = await _context.User.FindAsync(model.UserID);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Người dùng không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            // Xử lý upload ảnh
            if (avatarImgFile != null && avatarImgFile.Length > 0)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarImgFile.FileName);
                    string userPath = Path.Combine(wwwRootPath, "Images", "userAvt");

                    if (!Directory.Exists(userPath))
                    {
                        Directory.CreateDirectory(userPath);
                    }

                    string fullPath = Path.Combine(userPath, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await avatarImgFile.CopyToAsync(fileStream);
                    }

                    user.avatarImg = $"/Images/userAvt/{fileName}";
                    Response.Cookies.Append("avtImg", $"/Images/userAvt/{fileName}", new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        HttpOnly = false,
                        Secure = false,
                        SameSite = SameSiteMode.Lax
                    });
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Lỗi khi upload ảnh: {ex.Message}";
                    return View(model);
                }
            }

            // Cập nhật thông tin người dùng
            user.UserName = model.UserName;
            user.UserSurname = model.UserSurname;
            user.Phone = model.Phone;
            user.gender = model.gender;
            user.birthday = model.birthday;            

            await _dbContext.SaveChangesAsync();


            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Index", new { id = user.UserID });
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tìm người dùng theo email
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);

                // Kiểm tra xem người dùng có tồn tại và mật khẩu có hợp lệ không
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    // Đăng nhập thành công, tạo cookie xác thực
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email), // Sử dụng email làm tên
                new Claim("UserId", user.UserID.ToString()) // Lưu thông tin Id của người dùng
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true, // Cookie tồn tại sau khi đóng trình duyệt
                            ExpiresUtc = DateTime.UtcNow.AddDays(7) // Đặt thời gian hết hạn của cookie (7 ngày)
                        });

                    // Tạo cookie "UserEmail" chứa email người dùng
                    Response.Cookies.Append("UserEmail", user.Email, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax
                    });

                    Response.Cookies.Append("avtImg", user.avatarImg, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        HttpOnly = false,
                        Secure = false, 
                        SameSite = SameSiteMode.Lax
                    });
                    var checkUser = await _dbContext.User.FirstOrDefaultAsync(x => x.Email == user.Email);

                    if (string.IsNullOrEmpty(checkUser.UserName) || string.IsNullOrEmpty(checkUser.UserSurname))
                    {
                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        var checkUserToPayment = await _dbContext.User.FirstOrDefaultAsync(x => x.Email == model.Email);
                        Console.WriteLine("event id" + CheckoutController.EventId);
                        if (checkUserToPayment.status == "vertify" && CheckoutController.EventId != -1)
                        {
                            return RedirectToAction("Booking", "Checkout", new { eventId = CheckoutController.EventId });
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Nếu email hoặc mật khẩu không chính xác, thông báo lỗi
                    ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác.");
                }
            }

            return View(model); // Trả về lại trang đăng nhập với lỗi nếu có
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                    // Kiểm tra email đã tồn tại chưa
                    var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email is already in use.");
                        return View(model);
                    }

                    // Hash mật khẩu
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                    // Tạo đối tượng người dùng mới
                    var newUser = new User
                    {
                        Email = model.Email,
                        Password = hashedPassword,
                        avatarImg = "https://media.istockphoto.com/id/1341046662/vector/picture-profile-icon-human-or-people-sign-and-symbol-for-template-design.jpg?s=612x612&w=0&k=20&c=A7z3OK0fElK3tFntKObma-3a7PyO8_2xxW0jtmjzT78=",
                        status = "unvertify"
                        // Gán các thuộc tính khác nếu cần
                    };

                // Thêm người dùng vào database
                _context.User.Add(newUser);
                    await _context.SaveChangesAsync();

                    // Chuyển hướng tới trang đăng nhập
                    return RedirectToAction("Login", "Account");
            }

            // Trả lại view với các lỗi
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            // Xóa cookie xác thực
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Xóa cookie "UserEmail"
            Response.Cookies.Delete("UserEmail");
            Response.Cookies.Delete("avtImg");

            // Chuyển hướng người dùng đến trang đăng nhập (hoặc trang khác tùy ý)
            return RedirectToAction("Index", "Home");
        }
        public IActionResult VerifyEmail()
        {

           return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModels model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu email đã tồn tại
                var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser == null)
                {
                    // Nếu chưa tồn tại người dùng, trả về lỗi
                    ModelState.AddModelError("Email", "Email is not in use.");
                    return View(model);
                }
                TempData["Email"] = model.Email;
                // Nếu email tồn tại, chuyển đến trang ChangePassword
                return RedirectToAction("ChangePassword", "Account");
            }

            // Nếu ModelState không hợp lệ, trả về view với các lỗi
            return View(model);
        }
        public IActionResult ChangePassword()
        {
            // Lấy email từ TempData và gán vào model
            var model = new ChangePasswordViewModels
            {
                Email = TempData["Email"]?.ToString()  // Lấy email từ TempData
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModels model)
        {
            if (ModelState.IsValid)
            {
                // Tìm người dùng theo email
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("Email", "Email not found.");
                    return View(model);
                }

                // Hash mật khẩu mới
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);

                // Cập nhật mật khẩu mới cho người dùng
                user.Password = hashedPassword;

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();

                // Chuyển hướng người dùng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }
    

    }
}
