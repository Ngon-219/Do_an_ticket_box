using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_ticket_box.Models
{
    [Table("Report")]
    public class Report
    {
        [Key]
        public int Report_ID { get; set; }
        [ForeignKey("User")]
        public int User_ID { get; set; }
        [ForeignKey("Event")]
        public int Event_ID { get; set; }
        [Column("Report", TypeName = "int")]
        public int rate { get; set; }
        [Column("Comment", TypeName = "nvarchar(max)")]
        public string comment { get; set; }
        [Column("Created", TypeName = "timestamp")]
        public DateTime Created { get; set; } = DateTime.Now;
        public User User { get; set; }
        public Event Event { get; set; }
    }
}
