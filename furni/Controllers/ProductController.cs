using furni.Data;
using furni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

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
        
        private readonly ILogger<ProductController> _logger;
        private readonly ApplicationDbContext _context;
        private const int PageSize = 16;
        public ProductController(ILogger<ProductController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products
                .Include(p => p.Specification)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public async Task<IActionResult> ShowProduct(int pageNumber = 1/*, string sortBy = "name"*/)
        {
            IQueryable<Product> query = _context.Products;

            // Áp dụng sắp xếp dựa trên tham số 'sortBy'
            //switch (sortBy)
            //{
            //    case "price":
            //        query = query.OrderBy(p => p.Price);
            //        break;
            //    case "product":
            //        query = query.OrderBy(p => p.Name);
            //        break;
            //    case "name":
            //        break;
            //    default:
            //        break;
            //}

            var products = await query
                         .Skip((pageNumber - 1) * PageSize)
                         .Take(PageSize)
                         .ToListAsync();

            // Tính toán tổng số sản phẩm và tổng số trang như trước
            var totalProducts = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize);

            var childCategoryNames = await _context.Categories
                                       .Where(c => c.ParentId != null)
                                       .Select(c => c.Name)
                                       .ToListAsync();

            // Chuẩn bị và trả về ViewModel
            var viewModel = new ProductModel
            {
                TotalProduct = totalProducts,
                Products = products,
                PageNumber = pageNumber,
                TotalPages = totalPages,
                ChildCategoryNames = childCategoryNames
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


        public IActionResult Thanks()
        {
            return View();
        }

    }
}
