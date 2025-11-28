using Explorevia.Models;

namespace Explorevia.IRepository
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAllBookings();
        Booking GetBooking(int id);
        void AddBooking(Booking booking);
        void UpdateBookingStatus(int bookingId, string status);
    }
}
