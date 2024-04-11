﻿using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinitions.Role_Admin + "," + SystemDefinitions.Role_Employee)]
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IGenericRepository<OrdersModel, int> _orderRepo;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrdersController> _logger;
        public readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, IGenericRepository<OrdersModel,int> orderRepo, UserManager<ApplicationUser> userManager, ILogger<OrdersController> logger)
        {
            _orderRepo = orderRepo;
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var order = await _orderRepo.GetAllAsync();

                var currentUser = await _userManager.GetUserAsync(User);
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, SystemDefinitions.Role_Admin);
                ViewBag.IsAdmin = isAdmin;

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
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var order = await _orderRepo.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching order for update: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, OrdersModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _orderRepo.UpdateAsync(id, model);
                TempData["SuccessMessage"] = $"Order added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating order: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the order");
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index");
            }

            // Pass the order to a view which will show the confirmation dialog
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = SystemDefinitions.Role_Admin)]
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
    }
}
