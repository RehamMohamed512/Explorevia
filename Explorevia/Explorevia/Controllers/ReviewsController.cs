using Explorevia.Helpers;
using Explorevia.Models;
using ExploreVia.Services;

//using ExploreVia.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Explorevia.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly AppDbContext _context;

        public ReviewsController(IHotelService hotelService, AppDbContext context)
        {
            _hotelService = hotelService;
            _context = context;
        }

        [HttpPost]
        public IActionResult Add(int hotelId, int rating, string comment)
        {
            var hotel = _hotelService.GetById(hotelId);
            if (hotel == null) { NotificationHelper.Error(this, "Hotel not found!"); return RedirectToAction("Index", "Home"); }

            var review = new Review
            {
                HotelId = hotelId,
                UserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value),
                Rating = rating,
                Comment = comment
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();
            NotificationHelper.Success(this, "Review submitted successfully!");
            return RedirectToAction("Details", "Hotels", new { id = hotelId });
        }
    }
}
