namespace Do_an_ticket_box.ViewModels
{
    public class MyTicketVM
    {
        public string EventName { get; set; }
        public DateTime date { get; set; }
        public TimeOnly timeStart { get; set; }
        public int Ordercode { get; set; }
        public string status { get; set; }
        public string location { get; set; }
        public TimeOnly timeEnd { get; set; }

        public int Day => date.Day;
        public int Month => date.Month;
        public int Year => date.Year;

        internal object GetTicketsByStatus(string status)
        {
            throw new NotImplementedException();
        }

        //internal object GetTicketsByStatus(string status)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
