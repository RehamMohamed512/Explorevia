using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.Models;
using Explorevia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Licenses;
using System.Text.Json;

namespace Explorevia.Controllers
{
    public class RequestController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFileUploadService _fileUploadService;
        private readonly UserManager<ApplicationUser> _userManager;
        public RequestController(AppDbContext appDbContext, IFileUploadService fileUploadService,
            UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
        }
        public async Task<IActionResult> AddHotel(HotelOwnerRegisterViewModel hotel)
        {
            if(ModelState.IsValid)
            {
                
                var hotelLicenseUrl = await _fileUploadService.UploadFileAsync(hotel.HotelLicense);
                var ownerIdCardUrl = await _fileUploadService.UploadFileAsync(hotel.OwnerIdCard);
                var imgPath = new List<string>();
                if(hotel.Images != null )
                {
                    foreach (var img in hotel.Images)
                    {
                        var imgUrl = await _fileUploadService.UploadFileAsync(img);
                        if (imgUrl != null)
                        {
                            imgPath.Add(imgUrl);
                        }
                    }
                }
                var request = new HotelRegistrationRequest
                {
                    OwnerName = hotel.OwnerName,
                    HotelName = hotel.HotelName,
                    Email = hotel.Email,
                    Password = hotel.Password,
                    Phone = hotel.PhoneNumber,
                    Description = hotel.Description,
                    City = hotel.City,
                    Country = hotel.Country,
                    Address = hotel.Address,
                    Rating = hotel.Rating,
                    HotelLicensePath = hotelLicenseUrl,
                    OwnerIdCardPath = ownerIdCardUrl,
                    ImagesJson = JsonSerializer.Serialize(imgPath)
                };
                await _appDbContext.HotelRegistrationRequests.AddAsync(request);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
