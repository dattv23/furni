using furni.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var product = _context.Products.ToList();
            return View(product);
        }
        public IActionResult Create()
        {
            var categories = _context.Categories
                         .Where(c => c.ParentId != null) 
                         .Select(c => new { Id = c.Id, Name = c.Name })
                         .ToList();
            ViewBag.Categories = categories;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Product added successfully." });
            }

            // Trả về lỗi validation dưới dạng JSON nếu ModelState không hợp lệ
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Validation failed.", errors });
        }

        public IActionResult Delete()
        {
            var product = _context.Products.ToList();
            return View(product);
        }
        public IActionResult Update()
        {
            var product = _context.Products.ToList();
            return View(product);
        }
    }
}
