using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.Models;
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
<<<<<<< HEAD

=======
        [HttpGet("requests")]
>>>>>>> 380c7a80300a8409f8dd8b5b23a9342774243374
        public IActionResult GetRequests()
        {
            var requests = _adminRepository.Requests();
            if(requests != null)
            {
                return View("Requests", requests);
            }
            return NotFound("No pending requests found");

        }
<<<<<<< HEAD
        //[HttpGet]
        //public async Task<IActionResult> GetRequests()
        //{
        //    var request = _adminRepository.Requests(); 


        //}
    }   
=======

        [HttpGet]
        public async Task<List<Hotel>> GetAllHotels()
        {
            var hotels =await _adminRepository.GetAllHotels();
            return hotels;
        }

        [HttpGet("requests/{id}")]
        public IActionResult GetRequestDetails(int id)
        {
            var request = _adminRepository.GetDetails(id);
            if(request != null)
            {
                return View("RequestDetails", request);
            }
            return NotFound("Request not found");
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var result =await _adminRepository.ApproveRequest(requestId);
            if(!result)
            {
                return BadRequest("Unable to approve request");
            }
            return RedirectToAction("GetRequests");
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var result = await _adminRepository.RejectRequest(requestId);
            if(!result)
            {
                return BadRequest("Unable to reject request");
            }
            return RedirectToAction("GetRequests");
        }


    }
>>>>>>> 380c7a80300a8409f8dd8b5b23a9342774243374
}
