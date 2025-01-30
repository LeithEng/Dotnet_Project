using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
