using Explorevia.Helpers;
using Explorevia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Explorevia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Home Page — Featured Hotels
        public async Task<IActionResult> Index()
        {
            var featuredHotels = await _context.Hotels //retrieving all hotels 
                .Include(h => h.Reviews)
                .OrderByDescending(h => h.Rating)
                .Take(6)
                .ToListAsync();

            return View(featuredHotels);
        }

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

        //  Contact Page
        public IActionResult Contact()
        {
            return View();
        }

    }
}
