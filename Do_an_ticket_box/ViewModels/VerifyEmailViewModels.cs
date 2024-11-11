using System.ComponentModel.DataAnnotations;

namespace Do_an_ticket_box.ViewModels
{
    public class VerifyEmailViewModels
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
