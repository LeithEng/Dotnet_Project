using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class commentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
