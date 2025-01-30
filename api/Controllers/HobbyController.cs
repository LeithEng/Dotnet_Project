using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class HobbyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
