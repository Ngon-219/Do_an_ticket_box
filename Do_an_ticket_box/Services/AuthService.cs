using Do_an_ticket_box.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace Do_an_ticket_box.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService( ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>(); // Khởi tạo PasswordHasher với kiểu User
        }

        public bool Authenticate(string email, string password)
        {
            var user = _context.User.SingleOrDefault(u => u.Email == email);

            if (user == null)
            {
                return false;
            }

            // So sánh mật khẩu đã băm
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
