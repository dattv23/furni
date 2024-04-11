using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinitions.Role_Admin + "," + SystemDefinitions.Role_Employee)]
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
