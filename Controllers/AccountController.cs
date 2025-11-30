using Explorevia.ViewModels;
using Explorevia.Helpers;
using Explorevia.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Explorevia.Models;
using NuGet.Packaging.Signing;

namespace Explorevia.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAuthRepository authRepository, SignInManager<ApplicationUser> signInManager)
        {
            _authRepository = authRepository;
            _signInManager = signInManager;
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
                NotificationHelper.Error(this, "Registration failed, Please fill all the required fields");
                return View("Register",register);
            }

            var isCreated = await _authRepository.RegisterUser(register);
            if (!isCreated)
            {
                NotificationHelper.Error(this, "Registration failed, Email already exists");
                return View("Register",register);
            }

            NotificationHelper.Success(this, "Registration Successful, Please login");
            return RedirectToAction("Login","Account");
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
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            if (!ModelState.IsValid)
            {
                NotificationHelper.Error(this, "Login failed, Please fill all the required fields");
                return View("Login", loginVm);
            }

            var login = await _authRepository.Login(loginVm);

            if (login == "Envalid Email or Password")
            {
                NotificationHelper.Error(this, "Login failed, Envalid Email or Password");
                return View("Login", loginVm);
            }
            switch (login)
            {
                case "HotelOwner":
                    NotificationHelper.Success(this, "Login Successful");
                    return RedirectToAction("OwnerDashboard","Home");

                case "Admin":
                    NotificationHelper.Success(this, "Login Successful");
                    return RedirectToAction("GetRequests", "Admin");

                case "User":
                    NotificationHelper.Success(this, "Login Successful");
                    return RedirectToAction("Index", "Home");
            }
            NotificationHelper.Error(this, "Login failed, Please try again");
            return View("Login", loginVm);

        }

        //-----------------------------------------------------

        // Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
           // HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Identity.Application"); 
            Response.Cookies.Delete(".AspNetCore.Cookies");
            NotificationHelper.Success(this, "Logout Successful");
            return RedirectToAction("Login", "Account");


        }

        //-----------------------------------------------------
        // ID Verification using ML model
        [HttpPost]
        public async Task<IActionResult> CheckID(IFormFile idImage)
        {
            if (idImage == null || idImage.Length == 0)
                return Content("Please upload ID image");

            byte[] imageBytes;

            using (var ms = new MemoryStream())
            {
                await idImage.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var input = new ExploreviaML.ModelInput()
            {
                ImageSource = imageBytes
            };

            var result = ExploreviaML.Predict(input);

            return Content("Result: " + result.PredictedLabel);
        }

        //-----------------------------------------------------
        


    }
}
