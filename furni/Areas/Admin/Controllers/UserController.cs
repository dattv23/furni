using furni.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var user = _context.Users.ToList();
            return View(user);
        }
        public IActionResult Delete()
        {
            var user = _context.Users.ToList();
            return View(user);
        }
        public IActionResult Create()
        {
            var user = _context.Users.ToList();
            return View(user);
        }
        public IActionResult Update()
        {
            var user = _context.Users.ToList();
            return View(user);
        }
    }
}
