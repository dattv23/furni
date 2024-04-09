using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinications.Role_Admin)]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IGenericRepository<ProductModel, int> _productRepo;
        private readonly ILogger<UserController> _logger;
        public ProductController(IGenericRepository<ProductModel, int> productRepo, ILogger<UserController> logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productRepo.GetAllAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                // Consider returning an appropriate error view or a response
                return View("Error"); // Make sure to have an Error view to show error details or messages.
            }
        }
    }
}
