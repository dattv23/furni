using furni.Areas.Admin.Models;
using furni.Interfaces;
using furni.Models;
using furni.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Make sure you have this using statement for ILogger
using System;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinitions.Role_Admin)]
    [Area("Admin")]
    public class CouponController : Controller
    {
        // Dependencies are injected here. The repository for accessing coupon data and a logger for logging.
        private readonly IGenericRepository<CouponModel, int> _couponRepo;
        private readonly ILogger<CouponController> _logger;

        // Constructor to inject dependencies
        public CouponController(IGenericRepository<CouponModel, int> couponRepo, ILogger<CouponController> logger)
        {
            _couponRepo = couponRepo;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var coupons = await _couponRepo.GetAllAsync();
                return View(coupons);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching coupons: {ex.Message}");
                // Optionally, redirect to an error view or return an error message
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            // Implement your Add view return logic here
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CouponModel couponModel)
        {
            if (!ModelState.IsValid)
            {
                return View(couponModel);
            }

            try
            {
                await _couponRepo.AddAsync(couponModel);
                TempData["SuccessMessage"] = $"Coupon {couponModel.Code} added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding coupon: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while adding the coupon");
                return View(couponModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var couponModel = await _couponRepo.GetByIdAsync(id);
                if (couponModel == null)
                {
                    return NotFound();
                }
                return View(couponModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching coupon for update: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CouponModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _couponRepo.UpdateAsync(id, model);
                TempData["SuccessMessage"] = $"Coupon {model.Code} updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating coupon: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the coupon");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var couponModel = await _couponRepo.GetByIdAsync(id);
                if (couponModel == null)
                {
                    return NotFound();
                }
                return View(couponModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching coupon for deletion: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _couponRepo.RemoveAsync(id);
                TempData["SuccessMessage"] = "Coupon deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting coupon: {ex.Message}");
                // Optionally, return an error view or redirect to a safe page
                return View("Error");
            }
        }
    }
}
