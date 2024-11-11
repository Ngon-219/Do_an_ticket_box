using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_ticket_box.Models
{
    [Table("Ticket")]
    public class Ticket
    {
        [Key]
        public int Ticket_ID { get; set; }
        [ForeignKey("Event")]
        public int? Event_ID { get; set; }
        public Event? Event { get; set; }
        [Column("Ticket_type", TypeName ="nvarchar(50)")]
        public string? Ticket_type { get; set; }
        [Column("Price", TypeName = "Decimal(10,2)")]
        public decimal? price { get; set; }
        [Column("Seat_number", TypeName = "int")]
        public int? seat_number { get; set; }
        [Column("Status", TypeName = "nvarchar(50)")]
        public string? status { get; set; }
        [Column("Seat_remain", TypeName = "int")]
        public int? seat_remain { get; set; }

        [Column("start_time", TypeName = "datetime2")]
        public DateTime start_time { get; set; }

    }
}

