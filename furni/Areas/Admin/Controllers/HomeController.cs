using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
