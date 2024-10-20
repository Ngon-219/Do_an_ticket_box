using Do_an_ticket_box.Models;
using Microsoft.EntityFrameworkCore;

namespace Do_an_ticket_box.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(e => e.Event_time)
                .HasConversion(
                    time => time.ToTimeSpan(),  // Chuyển đổi TimeOnly sang TimeSpan
                    timeSpan => TimeOnly.FromTimeSpan(timeSpan) // Chuyển đổi TimeSpan sang TimeOnly
                );
        }
    }

}
