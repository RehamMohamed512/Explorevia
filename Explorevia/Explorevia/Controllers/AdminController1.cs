using Explorevia.Helpers;
using Explorevia.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorevia.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController1 : Controller
    {
        IAdminRepository _adminRepository;
        public AdminController1(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        [HttpGet]
        public IActionResult Requests()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var req = _adminRepository.Requests();
            if (req == null)
            { NotificationHelper.Error(this, "No pending requests found."); 
            return BadRequest();
            }
            return Ok(req);

        }

    }
}
