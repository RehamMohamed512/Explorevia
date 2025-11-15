using Explorevia.Models;
///using ExploreVia.Web.Data;
//using ExploreVia.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploreVia.Services
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAllBookings();
        Booking GetBooking(int id);
        void AddBooking(Booking booking);
        void UpdateBookingStatus(int bookingId, string status);
    }

    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        public BookingService(AppDbContext context) => _context = context;

        public IEnumerable<Booking> GetAllBookings() => _context.Bookings.Include(b => b.User).Include(b => b.Room).ThenInclude(r => r.Hotel).ToList();
        public Booking GetBooking(int id) => _context.Bookings.Include(b => b.User).Include(b => b.Room).ThenInclude(r => r.Hotel).FirstOrDefault(b => b.Id == id);
        public void AddBooking(Booking booking) { _context.Bookings.Add(booking); _context.SaveChanges(); }
        public void UpdateBookingStatus(int bookingId, string status)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking != null) { booking.Status = status; _context.SaveChanges(); }
        }
    }
}
