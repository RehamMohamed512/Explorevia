using Explorevia.Helpers;
using Explorevia.Models;
using ExploreVia.Services;
//using ExploreVia.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Explorevia.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IHotelService _hotelService;

        public BookingsController(IBookingService bookingService, IHotelService hotelService)
        {
            _bookingService = bookingService;
            _hotelService = hotelService;
        }

        // User: Book room
        [HttpGet]
        public IActionResult Create(int roomId)
        {
            ViewBag.Room = _hotelService.GetAll().SelectMany(h => h.Rooms).FirstOrDefault(r => r.Id == roomId);
            return View();
        }

        [HttpPost]
        public IActionResult Create(int roomId, DateTime startDate, DateTime endDate)
        {
            var room = _hotelService.GetAll().SelectMany(h => h.Rooms).FirstOrDefault(r => r.Id == roomId);
            if (room == null) { NotificationHelper.Error(this, "Room not found!"); return RedirectToAction("Index", "Home"); }

            decimal totalPrice = (decimal)(endDate - startDate).TotalDays * room.Price;
            var booking = new Booking
            {
                RoomId = roomId,
                UserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
                StartDate = startDate,
                EndDate = endDate,
                TotalPrice = totalPrice,
                Status = "Pending"
            };
            _bookingService.AddBooking(booking);
            NotificationHelper.Success(this, "Booking created successfully!");
            return RedirectToAction("MyBookings");
        }

        // User: My bookings
        public IActionResult MyBookings()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var bookings = _bookingService.GetAllBookings().Where(b => b.UserId == userId);
            return View(bookings);
        }

        // Admin: Update status
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStatus(int bookingId, string status)
        {
            _bookingService.UpdateBookingStatus(bookingId, status);
            NotificationHelper.Success(this, "Booking status updated!");
            return RedirectToAction("Bookings", "Admin");
        }
    }
}
