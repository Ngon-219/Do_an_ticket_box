using Do_an_ticket_box.Models;
using System.ComponentModel.DataAnnotations;

namespace Do_an_ticket_box.Services
{
    public class EmailVerificationToken
    {
        [Key]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreateOnUtc { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
        public List<User> User { get; set; }
    }
}
