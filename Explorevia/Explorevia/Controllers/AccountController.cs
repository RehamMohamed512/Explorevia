using Explorevia.DTOs;
using Explorevia.Helpers;
using Explorevia.IRepository;
using Explorevia.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> RegisterUser(RegisterViewModel registerDto)
        {
            if (!ModelState.IsValid)
            {
                NotificationHelper.Error(this, "Registration failed, Please fill all the required fields");
                return View("Register",registerDto);
            }

            var isCreated = await _authRepository.RegisterUser(registerDto);
            if (!isCreated)
            {
                NotificationHelper.Error(this, "Registration failed, Email already exists");
                return View("Register",registerDto);
            }

            NotificationHelper.Success(this, "Registration Successful, Please login");
            return RedirectToAction("Login","Account");
        }

        //-----------------------------------------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterHotel() => View("RegisterHotel");
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterHotel(HotelOwnerRegisterViewModel registerDto)
        {
            if (!ModelState.IsValid)
            {
                NotificationHelper.Error(this, "Registration failed, Please fill all the required fields");
                return View("RegisterHotel", registerDto);
            }
            var isCreated = await _authRepository.RegisterHotelOwner(registerDto);
            if (!isCreated)
            {
                NotificationHelper.Error(this, "Registration failed, Email already exists");
                return View("RegisterHotel", registerDto);
            }
            NotificationHelper.Success(this, "Registration Successful, Please login");
            return RedirectToAction("Login", "Account");
        }

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
                NotificationHelper.Error(this, "Login failed, Please fill all the required fields");
                return View("Login");
            }

            var login = await _authRepository.Login(loginDTO);

            if (login == "Envalid Email or Password")
            {
                NotificationHelper.Error(this, "Login failed, Envalid Email or Password");
                return View("Login");
            }
            switch (login)
            {
                case "HotelOwner":
                    NotificationHelper.Success(this, "Login Successful");
                    return RedirectToAction("Index", "HotelOwnerDashboard");

                case "Admin":
                    NotificationHelper.Success(this, "Login Successful");
                    return RedirectToAction("Index", "AdminDashboard");

                case "User":
                    NotificationHelper.Success(this, "Login Successful");
                    return RedirectToAction("Index", "Home");
            }
            NotificationHelper.Error(this, "Login failed, Please try again");
            return View("Login",loginDTO);

        }

        //-----------------------------------------------------

        // Logout
        [HttpPost]
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
