using Explorevia.Helpers;
using Explorevia.Models;
using Explorevia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Explorevia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        // Home Page — Featured Hotels

        //
        public async Task<IActionResult> Index()
        {
            var featuredHotels = await _context.Hotels
                .Include(h => h.HotelImages)
                .Include(h => h.Reviews)
                .ToListAsync();
            return View(featuredHotels);
        }
        [Authorize(Roles ="User,Admin")]

        //-----------------------------------------------------
        // view explore
        
        public async Task< IActionResult >Explore()
        {
            var hotels = await _context.Hotels
            .Include(h => h.HotelImages)
             .ToListAsync();

            return View("Explore", hotels);
        }



        [HttpGet("/")]
        //  Hotel Details Page
        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _context.Hotels //retrieving all hotels 
                .Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
            {
                NotificationHelper.Error(this, "Hotel not found!");
                return RedirectToAction("Index", "Home");
            }

            return View(hotel);
        }


        //-----------------------------------------------------
        //view Contact Page

        [HttpGet]
        public IActionResult Contact()
        {
            return View("Contact");
        }

        //-----------------------------------------------------
        //view Destinations Page
        public async Task<IActionResult> Destinations()
        {
            var locations = await _context.Hotels
                .Select(h => h.City)
                .Distinct()
                .ToListAsync();

            return View(locations);
        }


        //-----------------------------------------------------
        //view Owner Dashboard Page
        public IActionResult OwnerDashboard()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var hotel = _context.Hotels
                .Include(h => h.HotelImages)
                .Include(h => h.Reviews)
                .FirstOrDefault(h => h.OwnerId == ownerId);

            var hotelVM = new HotelVieModel
            {
                HotelId = hotel?.Id,
                HotelName = hotel?.Name,
                Price = hotel?.PricePerNight ?? 0,
                Rating = hotel?.Rating.ToString(),
                Description = hotel?.Description,
                ExistingImages = hotel?.HotelImages.ToList()
            };

            return View(hotelVM);
        }



    }
}
