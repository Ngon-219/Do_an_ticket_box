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
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {     
            modelBuilder.Entity<Event>()
                .Property(e => e.Event_time)
                .HasConversion(
                    time => time.ToTimeSpan(),  // Chuyển đổi TimeOnly sang TimeSpan
                    timeSpan => TimeOnly.FromTimeSpan(timeSpan) // Chuyển đổi TimeSpan sang TimeOnly
                );
            modelBuilder.Entity<Event>()
                .Property(e => e.Event_time_end)
                .HasConversion(
                    time => time.ToTimeSpan(),  // Chuyển đổi TimeOnly sang TimeSpan
                    timeSpan => TimeOnly.FromTimeSpan(timeSpan) // Chuyển đổi TimeSpan sang TimeOnly
                );
             modelBuilder.Entity<Event>()
                .Property(e => e.created_at_time)
                .HasDefaultValueSql("GETDATE()"); // SQL Server; use NOW() for PostgreSQL
            modelBuilder.Entity<User>()
               .Property(u => u.Created_at)
               .HasColumnName("Created_at")
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Booking>()
                .Property(b => b.booking_time)
                .HasColumnName("Booking_time")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }

}

