using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Explorevia.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingRepository _bookingService;
        private readonly IHotelRepository _hotelService;

        public BookingsController(IBookingRepository bookingService, IHotelRepository hotelService)
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
            if (room == null)
            {
                NotificationHelper.Error(this, "Room not found!");
                return RedirectToAction("Index", "Home");
            }

            decimal totalPrice = (decimal)(endDate - startDate).TotalDays * room.Price;
            var booking = new Booking
            {
                RoomId = roomId,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
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
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var bookings = _bookingService.GetAllBookings().Where(b => b.UserId.Equals(userId));
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
