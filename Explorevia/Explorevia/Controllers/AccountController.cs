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
<<<<<<< HEAD
        public async Task<IActionResult> Login(LoginViewModel loginDTO)
=======
        public async Task<IActionResult> Login(LoginDTO loginDTO)
>>>>>>> 371f56d96280209b8db5c5c7f6bac9aa137b8cfb
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
                    string key = "team R2M2 in depi explorevia project";
                    var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                    var signCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim("Email", "RmRm@gmail.com"));

                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials:signCred ) ;

                    var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
                   

                    NotificationHelper.Success(this, "Login Successful");
<<<<<<< HEAD
                    Console.WriteLine("Login Successfully");
                    return RedirectToAction("Index", "Home");


=======
                    return Ok( RedirectToAction("Index", "Home"));
>>>>>>> 371f56d96280209b8db5c5c7f6bac9aa137b8cfb
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