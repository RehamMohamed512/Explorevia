//using Explorevia.Helpers;
//using Explorevia.Services;
//using ExploreVia.Services;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace Explorevia.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly IUserService _userService;

//        public AccountController(IUserService userService) { _userService = userService; }

//        [HttpGet] public IActionResult Login() => View();
//        [HttpPost]
//        public async Task<IActionResult> Login(string email, string password)
//        {
//            var user = await _userService.LoginAsync(email, password);
//            if (user == null) { NotificationHelper.Error(this, "Invalid credentials!"); return View(); }

//            var claims = new List<Claim>{
//                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
//                new Claim(ClaimTypes.Name,user.Name),
//                new Claim(ClaimTypes.Email,user.Email),
//                new Claim(ClaimTypes.Role,user.Role)
//            };
//            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
//            NotificationHelper.Success(this, "Login successful!");
//            return RedirectToAction("Index", "Home");
//        }

//        [HttpGet] public IActionResult Register() => View();
//        [HttpPost]
//        public async Task<IActionResult> Register(string name, string email, string password)
//        {
//            var user = await _userService.RegisterAsync(name, email, password);
//            if (user == null) { NotificationHelper.Error(this, "Registration failed!"); return View(); }
//            NotificationHelper.Success(this, "Registration successful! Please login.");
//            return RedirectToAction("Login");
//        }

//        [HttpPost]
//        public async Task<IActionResult> Logout()
//        {
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//            NotificationHelper.Success(this, "Logged out successfully!");
//            return RedirectToAction("Index", "Home");
//        }
//    }
//}
