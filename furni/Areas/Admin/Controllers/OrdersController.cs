using furni.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var Order = _context.Orders.ToList();
            return View(Order);
        }
        public IActionResult Delete()
        {
            var Order = _context.Orders.ToList();
            return View(Order);
        }
        public IActionResult Create()
        {
            var Order = _context.Orders.ToList();
            return View(Order);
        }
        public IActionResult Update()
        {
            var Order = _context.Orders.ToList();
            return View(Order);
        }
    }
}
