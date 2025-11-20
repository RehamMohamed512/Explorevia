using Explorevia.DTOs;
using Explorevia.Helpers;
using Explorevia.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Explorevia.Controllers
{
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
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
        public async Task<IActionResult> Login(LoginDTO loginDTO)
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
                    return Ok( RedirectToAction("Index", "Home"));
                }
            }
            else
            {
                NotificationHelper.Error(this, "Login failed, Please fill all the required fields");
                return Unauthorized();
            }

        }


    }
}