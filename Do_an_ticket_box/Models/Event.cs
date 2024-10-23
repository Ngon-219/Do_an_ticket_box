﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_ticket_box.Models
{
    [Table("Event")]
    public class Event
    {
        [Key]
        public int Event_ID { get; set; }
        [Column("Event_name", TypeName = "varchar(150)")]
        public string Event_Name { get; set;  }
        [Column("Event_date", TypeName = "Date")]
        public DateTime Event_date { get; set; }
        [Column("Event_time")]
        public TimeOnly Event_time { get; set; }
        [Column("Location", TypeName = "varchar(255)")]
        public string location { get; set; }
        [Column("Description", TypeName = "text")]
        public string description { get; set; }
        [Column("Total_tickets", TypeName = "int")]
        public int total_tickets { get; set; }
        [Column("Avaiable_tickets", TypeName = "int")]
        public int avaiable_ticket { get; set; }
        [Column("Created_at", TypeName = "timestamp")]
        public DateTime created_at { get; set; }
        public List<Ticket> Ticket { get; set; }
        public List<Report> Report { get; set; }
    }
}