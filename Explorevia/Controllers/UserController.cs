using Microsoft.AspNetCore.Mvc;

namespace Explorevia.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
