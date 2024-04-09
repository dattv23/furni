using furni.Areas.Admin.Models;
using furni.Data;
using furni.Interfaces;
using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinications.Role_Admin)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IGenericRepository<UserModel, string> _userRepo;
        private readonly ILogger<UserController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IGenericRepository<UserModel, string> userRepo, ILogger<UserController> logger, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _userRepo = userRepo;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userRepo.GetAllAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                // Consider returning an appropriate error view or a response
                return View("Error"); // Make sure to have an Error view to show error details or messages.
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // Fetch the list of roles
            ViewBag.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protect against CSRF attacks
        public async Task<IActionResult> Add(UserModel userModel, string SelectedRole)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userRepo.AddAsync(userModel);
                    var user = await _userManager.FindByEmailAsync(userModel.Email);
                    await _userManager.AddToRoleAsync(user, SelectedRole);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while adding a new user.");
                    ModelState.AddModelError("", "An unexpected error occurred while adding the user. Please try again.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(userModel);
        }

        public async Task<IActionResult> Update(string id)
        {
            try
            {
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                ViewBag.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving a user with ID {UserId}", id);
                return View("Error"); // As before, ensure you have an appropriate response or error view.
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, UserModel model, string SelectedRole)
        {
            try
            {
                if (id != model.Id)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    await _userRepo.UpdateAsync(id, model);
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var item in roles)
                    {
                        if (item != SelectedRole)
                        {
                            await _userManager.RemoveFromRoleAsync(user, item);
                            await _userManager.AddToRoleAsync(user, SelectedRole);
                        }
                    }
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating a user with ID {UserId}", id);
                return View("Error"); // Handle the error appropriately.
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("Attempted to delete a user with ID {UserId} that does not exist.", id);
                    return NotFound();
                }

                await _userRepo.RemoveAsync(id); // Implement this method in your repository
                _logger.LogInformation("User with ID {UserId} was deleted successfully.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a user with ID {UserId}.", id);
                return View("Error"); // Or, consider a more specific error handling approach
            }
        }
    }
}