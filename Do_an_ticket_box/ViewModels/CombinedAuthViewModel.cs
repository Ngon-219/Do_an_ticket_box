
namespace Do_an_ticket_box.ViewModels
{
    public class CombinedAuthViewModel
    {
        public LoginViewModel Login { get; set; }
        public RegisterViewModel Register { get; set; }
        public VerifyEmailViewModels VerifyEmail { get; set; }
        public ChangePasswordViewModels ChangePassword { get; set; }
    }
}
