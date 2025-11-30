using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.Models;
using Explorevia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Explorevia.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        //-----------------------------------------------------
        // Get all pending hotel registration requests to admin index view
        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _adminRepository.Requests();
            if (requests != null)
            {
                return View("AdminIndex", requests);

            }
            return NotFound("No pending requests found");

        }

        //-----------------------------------------------------
        // Get all hotels for admin dashboard

        [HttpGet]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _adminRepository.GetAllHotels();

            var model = new AdminDashboardViewModel
            {
                Hotels = hotels ?? new List<Hotel>()
            };

            return View("Dashboard", model);
        }

        //-----------------------------------------------------


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

        //-----------------------------------------------------
        // Approve or Reject hotel registration request
        [HttpGet("ApproveRequest/{requestId}")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var result = await _adminRepository.ApproveRequest(requestId);
            if (!result)
            {
                return BadRequest("Unable to approve request");
            }
            return RedirectToAction("GetRequests");
        }

        [HttpGet("RejectRequest/{requestId}")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var result = await _adminRepository.RejectRequest(requestId);
            if (!result)
            {
                return BadRequest("Unable to reject request");
            }
            return RedirectToAction("GetRequests");
        }

        //-----------------------------------------------------
        // Delete hotel by admin from dashboard

        [HttpPost]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var result = await _adminRepository.DeleteHotel(id);

            if (result)
            {
                return RedirectToAction("GetAllHotels");
            }
            else
            {
                return BadRequest("Unable to delete hotel");
            }

            return RedirectToAction("GetAllHotels");
        }

    }
}
