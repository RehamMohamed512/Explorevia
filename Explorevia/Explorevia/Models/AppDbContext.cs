using Microsoft.EntityFrameworkCore;

namespace Explorevia.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override  void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Booking → Hotel (Fix cascade problem)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Hotel)
                .WithMany(h => h.Bookings)
                .HasForeignKey(b => b.HotelId)
                .OnDelete(DeleteBehavior.NoAction);

            // Booking → Room
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            // Booking → User
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Hotel → User (Owner)
            modelBuilder.Entity<Hotel>()
              .HasOne(h => h.Owner)
              .WithMany()
              .HasForeignKey(h => h.OwnerId)
              .OnDelete(DeleteBehavior.NoAction);

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<HotelImage> HotelImages { get; set; }
        public virtual DbSet<HotelRegistrationRequest> HotelRegistrationRequests { get; set; }
    }
}
