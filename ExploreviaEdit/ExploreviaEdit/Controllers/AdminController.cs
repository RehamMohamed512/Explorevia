using ExploreviaEdit.Helpers;
using ExploreviaEdit.IRepository;
using ExploreviaEdit.Models;
using ExploreviaEdit.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExploreviaEdit.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // Admin Dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var pendingRequests = await _adminRepository.GetPendingRequests();
            var hotels = await _adminRepository.GetHotels();

            var model = new AdminDashboardViewModel
            {
                PendingRequests = pendingRequests,
                Hotels = hotels
            };

            return View(model);
        }

        // Approve hotel request
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var success = await _adminRepository.ApproveRequest(id);
            if (success)
                NotificationHelper.Success(this, "Request approved and hotel created.");
            else
                NotificationHelper.Error(this, "Failed to approve request.");

            return RedirectToAction("Dashboard");
        }

        // Reject hotel request
        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var success = await _adminRepository.RejectRequest(id);
            if (success)
                NotificationHelper.Success(this, "Request rejected.");
            else
                NotificationHelper.Error(this, "Failed to reject request.");

            return RedirectToAction("Dashboard");
        }

        // Add hotel manually
        [HttpGet("add-hotel")]
        public IActionResult AddHotel() => View();

        [HttpPost("add-hotel")]
        public async Task<IActionResult> AddHotel(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                NotificationHelper.Error(this, "Invalid hotel data.");
                return View(hotel);
            }

            await _adminRepository.AddHotel(hotel);
            NotificationHelper.Success(this, "Hotel added successfully.");
            return RedirectToAction("Dashboard");
        }

        // Edit hotel
        [HttpGet("edit-hotel/{id}")]
        public async Task<IActionResult> EditHotel(int id)
        {
            var hotel = await _adminRepository.GetHotelById(id);
            if (hotel == null)
            {
                NotificationHelper.Error(this, "Hotel not found.");
                return RedirectToAction("Dashboard");
            }
            return View(hotel);
        }

        [HttpPost("edit-hotel/{id}")]
        public async Task<IActionResult> EditHotel(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                NotificationHelper.Error(this, "Invalid hotel data.");
                return View(hotel);
            }

            var success = await _adminRepository.UpdateHotel(hotel);
            if (success)
                NotificationHelper.Success(this, "Hotel updated successfully.");
            else
                NotificationHelper.Error(this, "Failed to update hotel.");

            return RedirectToAction("Dashboard");
        }

        // Delete hotel
        [HttpPost("delete-hotel/{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var success = await _adminRepository.DeleteHotel(id);
            if (success)
                NotificationHelper.Success(this, "Hotel deleted successfully.");
            else
                NotificationHelper.Error(this, "Failed to delete hotel.");

            return RedirectToAction("Dashboard");
        }
    }
}
