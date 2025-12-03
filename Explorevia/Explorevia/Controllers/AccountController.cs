using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.ViewModels;
using Explorevia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Explorevia.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthRepository _authRepository;

        public AccountController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        //-----------------------------------------------------

        // Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterUser() => View("Register");

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please fill all required fields correctly." });
            }

            var isCreated = await _authRepository.RegisterUser(register);
            if (!isCreated)
            {
                return Json(new { success = false, message = "Email already exists. Please login instead." });
            }

            return Json(new { success = true, message = "Registration successful! Please login.", redirectUrl = Url.Action("Login", "Account") });
        }

        //-----------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterHotel() => View("UploadDocs");

        //-----------------------------------------------------

        // Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View("Login");

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please fill all the required fields" });
            }

            var login = await _authRepository.Login(loginDTO);

            switch (login)
            {
                case "UserNotFound":
                    return Json(new { success = false, message = "User not found. Please register first." });

                case "WrongPassword":
                    return Json(new { success = false, message = "Incorrect password. Please try again." });

                case "HotelOwner":
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "HotelOwnerDashboard") });

                case "Admin":
                    return Json(new { success = true, redirectUrl = Url.Action("GetRequests", "Admin") }); ;

                case "User":
                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });

                default:
                    return Json(new { success = false, message = "Login failed. Please try again." });
            }
        }

        //-----------------------------------------------------

        // Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var logout = await _authRepository.LogoutAsync(); 
            if (!logout)
            {
                NotificationHelper.Error(this, "Logout failed, Please try again");
                return RedirectToAction("Index", "Home");
            }

            NotificationHelper.Success(this, "Logout Successful");
            return RedirectToAction("Login", "Account");

        }

    }
}
