using Microsoft.AspNetCore.Mvc;

namespace furni.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
        public IActionResult ShowProducts()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }
        public IActionResult Comfirm()
        {
            return View();
        }

        public IActionResult Thanks()
        {
            return View();
        }
    }
}
