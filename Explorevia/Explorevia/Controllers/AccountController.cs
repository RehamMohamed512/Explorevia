using Explorevia.DTOs;
using Explorevia.Helpers;
using Explorevia.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Explorevia.Controllers
{
    public class AccountController : Controller
    {
        IAuthRepository _authRepository;
        public AccountController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        // Register
        [HttpGet]
        public IActionResult Register() => View("Register");


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _authRepository.RegisterUser(registerDto);
                if (!user)
                {
                    NotificationHelper.Error(this, "Registration failed, Email already exist");
                    return View("Register");
                }
                NotificationHelper.Success(this, "Registration Successful, Please login");
                return View("Login");
            }
            else
            {
                NotificationHelper.Error(this, "Registration failed, Please fill all the required fields");
                return View("Register");
            }
        }

        // Login
        [HttpGet]
        public IActionResult Login() => View("Login");

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Login(LoginViewModel loginDTO)
        { 
            if (ModelState.IsValid)
            {
                var login = await _authRepository.Login(loginDTO);
                if (!login)
                {
                    NotificationHelper.Error(this, "Login failed, Invalid email or password");
                    return View("Login");
                }
                else
                {
                    NotificationHelper.Success(this, "Login Successful");
                    Console.WriteLine("Login Successfully");
                    return RedirectToAction("Index", "Home");


                }
            }
            else
            {
                NotificationHelper.Error(this, "Login failed, Please fill all the required fields");
                return View("Login");
                
            }
           

        }
  
        public async Task<IActionResult> Logout()
        {
           var result =  _authRepository.LogoutAsync();
            if (result != null)
                return RedirectToAction("Login", "Account");
            else
            {
                ModelState.AddModelError("", "Logout failed");
                return View();
            }
               

        }



    }
}