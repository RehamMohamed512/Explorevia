using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult GetRequests()
        {
            return View();
        }
        //[HttpGet]
        //public async Task<IActionResult> GetRequests()
        //{
        //    var request = _adminRepository.Requests(); 


        //}
    }   
}
