using Do_an_ticket_box.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_ticket_box.ViewModels
{
    public class UpdateUserViewModel
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
       
        public string? UserSurname { get; set; }
      
        public string? Email { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Số điện thoại chỉ được chứa các ký tự số.")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được dài quá 20 ký tự.")]
        public string? Phone { get; set; }
            
        public string? avatarImg { get; set; }
      
        public string? gender { get; set; }
      
        public DateTime? birthday { get; set; }
    
    }
}
