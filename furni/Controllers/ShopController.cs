using furni.Data;
using furni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace furni.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        private readonly ApplicationDbContext _context;
        private const int PageSize = 12;
        public ShopController(ILogger<ShopController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products
                .Include(p => p.Specification)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public async Task<IActionResult> ShowProduct(int pageNumber = 1, int? categoryId = null)
        {
            IQueryable<Product> query = _context.Products;
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId );
            }


            var products = await query
                          .Where(p => (p.IsDeleted == false || p.IsDeleted == null))
                         .Skip((pageNumber - 1) * PageSize)
                         .Take(PageSize)
                         .ToListAsync();
            //var totalProducts = await _context.Products.CountAsync();
            var totalProducts = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);

            var childCategories = await _context.Categories
                         .Where(c => c.ParentId != null)
                         .Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name, ParentId = c.ParentId })
                         .ToListAsync();

            var parentCategories = await _context.Categories
                         .Where(c => (c.ParentId == null || c.Id == c.ParentId))
                         .Select(c => new CategoryViewModel { Id = c.Id, Name = c.Name })
                         .ToListAsync();

            // Chuẩn bị và trả về ViewModel
            var viewModel = new ProductViewModel
            {
                TotalProduct = totalProducts,
                Products = products,
                PageNumber = pageNumber,
                TotalPages = totalPages,
                ChildCategories = childCategories,
                ParentCategory = parentCategories
            };

            return View(viewModel);
        }




        public IActionResult Payment()
        {
            return View();
        }
        public IActionResult Comfirm()
        {
            return View();
        }
    }
}
