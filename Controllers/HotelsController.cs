// HotelController

using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.Models;
using Explorevia.Repository;
using Explorevia.ViewModels;


//using ExploreVia.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Explorevia.Controllers
{
    //CRUD FOR HOTELS
    public class HotelsController : Controller
    {
        private readonly IHotelRepository _hotelService;
        private readonly IWebHostEnvironment _env;
        private readonly IAuthRepository _authRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly AppDbContext _context;

        public HotelsController(IHotelRepository hotelService, IWebHostEnvironment env, IAuthRepository repo, AppDbContext context, IAdminRepository adminRepository)
        {
            _hotelService = hotelService;
            _env = env;
            _authRepository = repo;
            _context = context;
            _adminRepository = adminRepository;
        }
        //-----------------------------------------------------
        //hotel owner can edit hotel details
        [Authorize(Roles = "HotelOwner")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // know nully hotel id
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hotel =  _hotelService.GetById(id);
            if (hotel.OwnerId != ownerId)
                return Unauthorized();

            // check if hotel exists and belongs to owner
            if (hotel == null || hotel.OwnerId != ownerId)
            {
                NotificationHelper.Error(this, "Not allowed.");
                return RedirectToAction("OwnerDashboard", "Home");
            }
            // map to view model
            var newHotel = new HotelVieModel
            {
                HotelName = hotel.Name,
                HotelId = hotel.Id,
                Description = hotel.Description,
                Price = hotel.PricePerNight,
                Rating = hotel.Rating.ToString(),
                ExistingImages = hotel.HotelImages.ToList(),
            };
            return View("OwnerDashboard",newHotel);
        }

        //-----------------------------------------------------
        //hotel owner can edit hotel details
        [Authorize(Roles = "HotelOwner")]
        [HttpPost]
        public async Task< IActionResult> Edit(HotelVieModel hotel ,List<IFormFile>? Images)
        {
            // know nully hotel id
            if (!hotel.HotelId.HasValue)
                return BadRequest();

            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingHotel = _hotelService.GetById(hotel.HotelId.Value);

            if (existingHotel == null || existingHotel.OwnerId != ownerId)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                hotel.ExistingImages = existingHotel.HotelImages.ToList();
                return View("OwnerDashboard", hotel);
            }

            // update hotel details
            existingHotel.Name = hotel.HotelName;
            existingHotel.PricePerNight = hotel.Price;
            existingHotel.Description = hotel.Description;
            existingHotel.Rating =double.Parse( hotel.Rating);

            // -----------------------------
            // save new images
            // -----------------------------
            if (Images != null && Images.Any())
            {
                string folder = Path.Combine(_env.WebRootPath, "images/hotels");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                _context.HotelImages.RemoveRange(existingHotel.HotelImages);


                existingHotel.HotelImages = new List<HotelImage>();

                int index = 1;
                foreach (var img in Images)
                {
                    string fileName = $"{Guid.NewGuid()}_{index}{Path.GetExtension(img.FileName)}";
                    string path = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    existingHotel.HotelImages.Add(new HotelImage
                    {
                        HotelId = existingHotel.Id,
                        ImageUrl = "/images/hotels/" + fileName
                    });

                    index++;
                }
            }

            await _hotelService.UpdateAsync(existingHotel);
            NotificationHelper.Success(this, "Hotel updated successfully!");

            return RedirectToAction("OwnerDashboard", "Home");
        }




        [HttpGet("filter")]
        public async Task<IActionResult> Filter(string location, int minPrice, int maxPrice, double minRating)
        {
            var hotels = await _hotelService.FilterAsync(location, minPrice, maxPrice, minRating);
            return PartialView("_HotelsGrid", hotels); // partial view only for the hotel cards
        }

        //-----------------------------------------------------
        //complete hotel owner registration and send request to admin
        [HttpPost]
        public async Task<IActionResult> SendHotelRegistration(HotelOwnerRegisterViewModel hotel)
        {
            if (!ModelState.IsValid)
            {
                return Content("Invalid data. Please check the input fields.");
            }
            var result = await _authRepository.SendHotelRegisterRequest(hotel);
            if (result)
            {
                return Content("Hotel registration request sent successfully.");
            }
            else
            {
                NotificationHelper.Error(this, "Failed to send hotel registration request.");
                return Content("Failed to send hotel registration request.");
            }
        }
    }
}