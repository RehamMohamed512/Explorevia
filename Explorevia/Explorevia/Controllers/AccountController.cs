using Explorevia.DTOs;
using Explorevia.Helpers;
using Explorevia.IRepository;
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

        // Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View("Register");

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerDto)
        {
            if (!ModelState.IsValid)
            {
                NotificationHelper.Error(this, "Registration failed, Please fill all the required fields");
                return View("Register");
            }

            var user = await _authRepository.RegisterUser(registerDto);
            if (!user)
            {
                NotificationHelper.Error(this, "Registration failed, Email already exists");
                return View("Register");
            }

            NotificationHelper.Success(this, "Registration Successful, Please login");
            return View("Login");
        }

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

            if (!login)
            {
                NotificationHelper.Error(this, "Login failed, Invalid email or password");
                return View("Login");
            }

            NotificationHelper.Success(this, "Login Successful");
            return RedirectToAction("Index", "Home");
        }

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
