using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_ticket_box.Models
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public int Payment_ID { get; set; }
        [ForeignKey("Booking")]
        public int? Booking_ID { get; set; }
        [Column("Payment_time", TypeName = "timestamp")]
        public DateTime payment_time { get; set; }
        [Column("Amount_paid", TypeName = "decimal")]
        public decimal? amount_paid { get; set; }
        [Column("Status", TypeName = "nvarchar(20)")]
        public string? status { get; set; }
        public Booking? Booking { get; set; }
    }
}
