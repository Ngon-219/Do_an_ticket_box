using Do_an_ticket_box.Services;
using Microsoft.AspNetCore.Mvc;
using Do_an_ticket_box.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

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

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var firstUser = await _dbContext.User.FirstOrDefaultAsync();
                if (firstUser == null)
                {
                    return NotFound("Không có người dùng nào trong cơ sở dữ liệu.");
                }
                id = firstUser.UserID;
            }

            var user = await _dbContext.User.FindAsync(id);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, User user, IFormFile? avatarImg)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra nếu có ảnh mới được upload
                    if (avatarImg != null && avatarImg.Length > 0)
                    {
                        // Đảm bảo thư mục lưu trữ ảnh tồn tại
                        var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/avatars");
                        Directory.CreateDirectory(uploadDir);

                        // Tạo tên file duy nhất
                        var fileName = $"{Guid.NewGuid()}_{avatarImg.FileName}";
                        var filePath = Path.Combine(uploadDir, fileName);

                        // Lưu ảnh vào thư mục uploads/avatars
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await avatarImg.CopyToAsync(stream);
                        }

                        // Cập nhật đường dẫn của ảnh đại diện vào thuộc tính AvatarImg
                        user.avatarImg = $"/uploads/avatars/{fileName}";
                    }

                    // Cập nhật dữ liệu người dùng
                    _dbContext.Entry(user).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction("Index", new { id = user.UserID });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "Không thể cập nhật dữ liệu vào DB: " + ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }

            return View(user);
        }
    }
}
