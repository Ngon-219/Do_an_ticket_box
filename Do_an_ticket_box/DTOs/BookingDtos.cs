namespace Do_an_ticket_box.DTOs
{
    public class BookingDtos
    {
        public string? TicketName { get; set; }
        public TimeOnly? eventTime { get; set; }
        public DateTime? eventDate { get; set; }
        public string? location { get; set; }
        public int? Quanlity { get; set; }
    }
}
