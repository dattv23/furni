using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinitions.Role_Admin + "," + SystemDefinitions.Role_Employee)]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<CategoryModel, int> _categoryRepo;
        private readonly ILogger<CategoryController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger, IGenericRepository<CategoryModel, int> categoryrepo, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _categoryRepo = categoryrepo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var category = await _categoryRepo.GetAllAsync();
                ViewBag.Categories = await _categoryRepo.GetAllAsync();
                var currentUser = await _userManager.GetUserAsync(User);
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, SystemDefinitions.Role_Admin);
                ViewBag.IsAdmin = isAdmin;
                //ViewBag.Categories =category;
                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                // Consider returning an appropriate error view or a response
                return View("Error"); // Make sure to have an Error view to show error details or messages.
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepo.GetAllAsync();
            var filteredCategories = categories.Where(c => c.ParentId == null).ToList();
            ViewBag.CategoriesId = new SelectList(filteredCategories, "Id", "Name");
   
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryModel categoryModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryRepo.AddAsync(categoryModel);
                    TempData["SuccessMessage"] = $"Category {categoryModel.Name} was created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log the exception with context information
                    _logger.LogError(ex, "An error occurred while adding a new Category: {CategoryName}", categoryModel.Name);

                    // Add a more descriptive error message for users
                    ModelState.AddModelError("", "An unexpected error occurred while adding the Category. Please try again.");
                }
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "category not found.";
                    return RedirectToAction("Index");
                }

                // Fetch the list of categories
                var categories = await _categoryRepo.GetAllAsync();
                var filteredCategories = categories.Where(c => c.ParentId == null).ToList();
                ViewBag.Categories = new SelectList(filteredCategories, "Id", "Name");
                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving category details for ID {CategoryId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving category details. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    TempData["ErrorMessage"] = "Category ID mismatch.";
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    await _categoryRepo.UpdateAsync(id, model);
                    // Add a success message to TempData
                    TempData["SuccessMessage"] = $"Category {model.Name} was updated successfully.";
                    return RedirectToAction("Index"); 
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Category with ID {CategoryId}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the Category. Please try again.";
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "category not found.";
                return RedirectToAction("Index");
            }

            // Pass the category to a view which will show the confirmation dialog
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = SystemDefinitions.Role_Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Attempted to delete a category with ID {CategoryId} that does not exist.", id);
                    return NotFound();
                }

                await _categoryRepo.RemoveAsync(id); // Implement this method in your repository
                _logger.LogInformation("Category with ID {CategoryId} was deleted successfully.", id);

                // Add a success message to TempData
                TempData["SuccessMessage"] = $"Category {category.Name} was deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a category with ID {CategoryId}.", id);
                return View("Error"); // Or, consider a more specific error handling approach
            }
        }
    }
}
