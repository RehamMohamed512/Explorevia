// HotelController

using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.Models;
using Explorevia.Repository;
using Explorevia.ViewModels;


//using ExploreVia.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting;

namespace Explorevia.Controllers
{
    //CRUD FOR HOTELS
    public class HotelsController : Controller
    {
        private readonly IHotelRepository _hotelService;
        private readonly IWebHostEnvironment _env;
        private readonly IAuthRepository _authRepository;

        public HotelsController(IHotelRepository hotelService, IWebHostEnvironment env, IAuthRepository repo)
        {
            _hotelService = hotelService;
            _env = env;
            _authRepository = repo;
        }


        // Hotel details
        public IActionResult Details(int id)
        {
            var hotel = _hotelService.GetById(id);
            if (hotel == null) return NotFound();
            return View(hotel);
        }

        // Admin: Create hotel
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() => View();

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public IActionResult Create(Hotel hotel, IFormFile? file)
        //{
        //    if (!ModelState.IsValid) return View(hotel);

        //    if (file != null)
        //    {
        //        string folder = Path.Combine(_env.WebRootPath, "images/hotels");
        //        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        //        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        //        string path = Path.Combine(folder, fileName);
        //        using var stream = new FileStream(path, FileMode.Create);
        //        file.CopyTo(stream);

        //        hotel.HotelImages = new List<HotelImage> { new HotelImage { ImageUrl = "/images/hotels/" + fileName } };
        //    }

        //    _hotelService.AddHotel(hotel);
        //    //_hotelService.SaveChanges();
        //    NotificationHelper.Success(this, "Hotel created successfully!");
        //    return RedirectToAction("Hotels", "Admin");
        //}

        // Admin: Edit hotel
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)

        {
            var hotel = _hotelService.GetById(id);
            if (hotel == null) return NotFound();
            return View(hotel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Hotel hotel, IFormFile? file)
        {
            if (!ModelState.IsValid) return View(hotel); //checking that hotel obj satisfies the data annotations specified in model or not 

            if (file != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "images/hotels");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string path = Path.Combine(folder, fileName);
                using var stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);

                hotel.HotelImages = new List<HotelImage> { new HotelImage { ImageUrl = "/images/hotels/" + fileName } };
            }

            _hotelService.UpdateHotel(hotel);
            NotificationHelper.Success(this, "Hotel updated successfully!");
            return RedirectToAction("Hotels", "Admin");
        }

        // Admin: Delete hotel
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _hotelService.DeleteHotel(id);
            NotificationHelper.Success(this, "Hotel deleted successfully!");
            return RedirectToAction("Hotels", "Admin");
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Filter(string location, int minPrice, int maxPrice, double minRating)
        {
            var hotels = await _hotelService.FilterAsync(location, minPrice, maxPrice, minRating);
            return PartialView("_HotelsGrid", hotels); // partial view only for the hotel cards
        }


        [HttpPost]
        public async Task<IActionResult> SendHotelRegistration(HotelOwnerRegisterViewModel hotel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data. Please check the input fields.";
                return RedirectToAction("UploadDocs");
            }
            var result = await _authRepository.SendHotelRegisterRequest(hotel);
            if (result)
            {
                TempData["Success"] = "Hotel registration request sent successfully!";
                return RedirectToAction("UploadDocs");
            }
            else
            {
                TempData["Error"] = "Failed to send hotel registration request.";
                return RedirectToAction("UploadDocs");
            }
        }
        [HttpGet]
        public IActionResult UploadDocs()
        {
            return View();
        }

    }

}