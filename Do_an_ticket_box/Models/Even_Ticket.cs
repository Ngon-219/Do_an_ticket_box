using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_an_ticket_box.Models
{
    
        public class EventTicketViewModel
        {
            public int Event_ID { get; set; }
            public string Event_Name { get; set; }
            public decimal? Price { get; set; }
            public DateTime Event_Date { get; set; }
            public string? Event_image { get; internal set; }
    }
    
}
