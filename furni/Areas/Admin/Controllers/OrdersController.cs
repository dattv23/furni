using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static NuGet.Packaging.PackagingConstants;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinications.Role_Admin + "," + SystemDefinications.Role_Employee)]
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IGenericRepository<OrdersModel, int> _orderRepo;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdersController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<UserModel, string> _userRepo;
        public OrdersController( RoleManager<IdentityRole> roleManager,ApplicationDbContext context, IGenericRepository<OrdersModel,int >orderRepo, UserManager<ApplicationUser> userManager, ILogger<OrdersController> logger, IGenericRepository<UserModel, String> userRepo)
        {
            _orderRepo = orderRepo;
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _userRepo = userRepo;
            _roleManager = roleManager;

        }
        public async Task<IActionResult> Index()
        {
            
            try
            {
                var order = await _orderRepo.GetAllAsync();
                ViewBag.orders = await _orderRepo.GetAllAsync();

                var currentUser = await _userManager.GetUserAsync(User);
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, SystemDefinications.Role_Admin);
                ViewBag.IsAdmin = isAdmin;

                //ViewBag.Categories =category;
                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                // Consider returning an appropriate error view or a response
                return View("Error"); // Make sure to have an Error view to show error details or messages.
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "category not found.";
                return RedirectToAction("Index");
            }

            // Pass the order to a view which will show the confirmation dialog
            return View(order);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = SystemDefinications.Role_Admin)]
        [ValidateAntiForgeryToken]
        virtual
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var order = await _orderRepo.GetByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Attempted to delete a order with ID {OrderId} that does not exist.", id);
                    return NotFound();
                }

                await _orderRepo.RemoveAsync(id); // Implement this method in your repository
                _logger.LogInformation("Order with ID {OrderId} was deleted successfully.", id);

                // Add a success message to TempData
                TempData["SuccessMessage"] = $"Order {order.UserId} was deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a Order with ID {Order}.", id);
                return View("Error"); // Or, consider a more specific error handling approach
            }
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
