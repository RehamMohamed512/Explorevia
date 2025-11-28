using Explorevia.Helpers;
using Explorevia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
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
        [HttpGet]
        // Home Page — Featured Hotels
        public async Task<IActionResult> Index()
        {
            var featuredHotels = await _context.Hotels
                .Include(h => h.HotelImages)
                .Include(h => h.Reviews)
                .ToListAsync();
            return View(featuredHotels);
        }
        // view explore now (apply filters +hotels)
        public async Task< IActionResult >Explore()
        {
            var hotels = await _context.Hotels
         .Include(h => h.HotelImages)
         .ToListAsync();

            return View("Explore", hotels);
        }


        public ActionResult Profile()
        {
            return View();
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

        //  Contact Page
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult OwnerDashboard()
        {
            return View("~/Views/Account/OnwerDashboard.cshtml");
        }
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
