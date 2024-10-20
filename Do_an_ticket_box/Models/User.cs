/*using Microsoft.EntityFrameworkCore;*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*using System.Diagnostics.Contracts;*/

namespace Do_an_ticket_box.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Column("Name", TypeName = "varchar(100)")]
        public string UserName { get; set; }
        [Column("Surname", TypeName = "varchar(100)")]
        public string UserSurname { get; set; }
        [Column("Email", TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column("Phone", TypeName = "varchar(20)")]
        public string Phone { get; set; }
        [Column("Password", TypeName = "varchar(255)")]
        public string Password { get; set; }
        [Column("Address", TypeName = "text")]
        public string Address { get; set; }
        [Column("Created_at", TypeName ="timestamp")]
        public DateTime Created_at { get; set; }
        [Column("Role", TypeName ="varchar(20)")]
        public string role { get; set; }
        [Column("Status", TypeName ="varchar(20)")]
        public string status { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Report> Reports { get; set; }

    }
}
