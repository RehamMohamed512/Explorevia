using Explorevia.Models;
using Microsoft.EntityFrameworkCore;

namespace Explorevia.Repository
{
    public class BookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Booking> GetAllBookings()
        {
           return _context.Bookings.Include(b => b.User).Include(b => b.Room).ThenInclude(r => r.Hotel).ToList();
        }
        public Booking GetBooking(int id) => _context.Bookings.Include(b => b.User).Include(b => b.Room).ThenInclude(r => r.Hotel).FirstOrDefault(b => b.Id == id);
        public void AddBooking(Booking booking) { _context.Bookings.Add(booking); _context.SaveChanges(); }
        public void UpdateBookingStatus(int bookingId, string status)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking != null) { booking.Status = status; _context.SaveChanges(); }
        }
    }
}
