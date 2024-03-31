using Microsoft.AspNetCore.Mvc;

namespace furni.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
