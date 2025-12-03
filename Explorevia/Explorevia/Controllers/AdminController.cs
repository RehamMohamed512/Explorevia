using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorevia.Controllers
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

        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _adminRepository.Requests();
            if (requests != null && requests.Any())
            {
                return View("AdminIndex", requests);
            }
            TempData["Info"] = "No pending requests found.";
            return View("AdminIndex", new List<HotelRegistrationRequest>());
        }

        [HttpGet]
        public async Task<List<Hotel>> GetAllHotels()
        {
            var hotels = await _adminRepository.GetAllHotels();
            return hotels;
        }

        [HttpGet("requests/{id}")]
        public IActionResult GetRequestDetails(int id)
        {
            var request = _adminRepository.GetDetails(id);
            if (request != null)
            {
                return View("RequestDetails", request);
            }
            return NotFound("Request not found");
        }

        [HttpGet("ApproveRequest/{requestId}")]  // ✅ Route parameter
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var result = await _adminRepository.ApproveRequest(requestId);
            if (!result)
            {
                TempData["Error"] = "Unable to approve request. Email may already exist.";
                return RedirectToAction("GetRequests");
            }

            TempData["Success"] = "Hotel request approved successfully!";
            return RedirectToAction("GetRequests");
        }

        [HttpGet("RejectRequest/{requestId}")]  // ✅ Route parameter
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var result = await _adminRepository.RejectRequest(requestId);
            if (!result)
            {
                TempData["Error"] = "Unable to reject request";
                return RedirectToAction("GetRequests");
            }

            TempData["Success"] = "Hotel request rejected successfully!";
            return RedirectToAction("GetRequests");
        }
    }
}