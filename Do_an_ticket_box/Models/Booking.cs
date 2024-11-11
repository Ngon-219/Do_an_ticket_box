using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_ticket_box.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        public int Booking_ID { get; set; }
        [ForeignKey("User")]
        public int? User_ID { get; set; }
        [ForeignKey("Event")]
        public int? Event_ID { get; set; }
        [ForeignKey("Ticket")]
        public int? Ticket_ID { get; set; }
        [Column("Booking_time", TypeName =  "timestamp")]
        public DateTime booking_time { get; set; }
        [Column("Total_amout", TypeName = "decimal")]
        public decimal? total_amout { get; set; }
        [Column("Status", TypeName = "nvarchar(20)")]
        public string? status { get; set; }
        public User? User { get; set; }
    }
}
