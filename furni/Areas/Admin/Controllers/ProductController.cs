using furni.Areas.Admin.Models;
using furni.Areas.Admin.Repositories;
using furni.Data;
using furni.Interfaces;
using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using furni.Services;

namespace furni.Areas.Admin.Controllers
{
    [Authorize(Roles = SystemDefinitions.Role_Admin + "," + SystemDefinitions.Role_Employee)]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IGenericRepository<ProductModel, int> _productRepo;
        private readonly IGenericRepository<CategoryModel, int> _categoryRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ImgurService _imgurService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IGenericRepository<ProductModel, int> productRepo, IGenericRepository<CategoryModel, int> categoryRepo, UserManager<ApplicationUser> userManager, ImgurService imgurService, ILogger<ProductController> logger)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _userManager = userManager;
            _imgurService = imgurService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productRepo.GetAllAsync();
                ViewBag.Categories = await _categoryRepo.GetAllAsync();
                var currentUser = await _userManager.GetUserAsync(User);
                var isAdmin = await _userManager.IsInRoleAsync(currentUser, SystemDefinitions.Role_Admin);
                ViewBag.IsAdmin = isAdmin;
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                // Consider returning an appropriate error view or a response
                return View("Error"); // Make sure to have an Error view to show error details or messages.
            }
        }

        // Display a single product
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _categoryRepo.GetAllAsync();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // Fetch the list of categories
            ViewBag.Categories = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductModel productModel, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageUrl != null)
                    {
                        //string imageUrl = await SaveImage(ImageUrl);
                        // Read the image file into a byte array
                        using (var memoryStream = new MemoryStream())
                        {
                            await ImageUrl.CopyToAsync(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();

                            // Upload the image to Imgur using ImgurService
                            string imageUrl = await _imgurService.UploadImageAsync(imageBytes);

                            // Assign the uploaded image URL to the product model
                            productModel.ImageUrl = imageUrl;
                        }
                    } else
                    {
                        productModel.ImageUrl = "None";
                    }
                    await _productRepo.AddAsync(productModel);
                    // Add a success message to TempData
                    TempData["SuccessMessage"] = $"Product {productModel.Name} was created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the exception with context information
                    _logger.LogError(ex, "An error occurred while adding a new product: {ProductName}", productModel.Name);

                    // Add a more descriptive error message for users
                    ModelState.AddModelError("", "An unexpected error occurred while adding the product. Please try again.");
                }
            }
            else
            {
                // If model state is not valid, it might be due to validation errors.
                // You may want to add logging for such cases as well.
                _logger.LogWarning("Invalid model state while trying to add a new product: {ProductName}", productModel.Name);
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Categories = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name");
            return View(productModel);
        }

        [HttpPost]
        public async Task<string> SaveImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(upload.FileName);
                var savePath = Path.Combine("wwwroot/images", upload.FileName);

                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }

                return "/images/" + fileName;
            }
            else
            {
                throw new ArgumentException("No image uploaded.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var fileExtension = Path.GetExtension(upload.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" }; // Adjust this list according to your requirements
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("", "Invalid file format. Only JPG, JPEG, PNG, or GIF are allowed.");
                    return RedirectToAction("Add");
                }
                var savePath = Path.Combine("wwwroot/images", upload.FileName);
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }
                return new JsonResult(new { path = "/images/" + upload.FileName });
            }
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                    return RedirectToAction("Index");
                }

                // Fetch the list of categories
                ViewBag.Categories = new SelectList(await _categoryRepo.GetAllAsync(), "Id", "Name");
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving product details for ID {ProductId}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving product details. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ProductModel model, IFormFile ImageUrl)
        {
            try
            {
                if (id != model.Id)
                {
                    TempData["ErrorMessage"] = "Product ID mismatch.";
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    if (ImageUrl != null)
                    {
                        //string imageUrl = await SaveImage(ImageUrl);
                        // Read the image file into a byte array
                        using (var memoryStream = new MemoryStream())
                        {
                            await ImageUrl.CopyToAsync(memoryStream);
                            byte[] imageBytes = memoryStream.ToArray();

                            // Upload the image to Imgur using ImgurService
                            string imageUrl = await _imgurService.UploadImageAsync(imageBytes);

                            // Assign the uploaded image URL to the product model
                            model.ImageUrl = imageUrl;
                        }
                    }
                    await _productRepo.UpdateAsync(id, model);
                    TempData["SuccessMessage"] = "Product updated successfully.";
                    // Add a success message to TempData
                    TempData["SuccessMessage"] = $"Product {model.Name} was updated successfully.";
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product with ID {ProductId}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the product. Please try again.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Authorize(Roles = SystemDefinitions.Role_Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Index");
            }

            // Pass the product to a view which will show the confirmation dialog
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = SystemDefinitions.Role_Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Attempted to delete a product with ID {ProductId} that does not exist.", id);
                    return NotFound();
                }

                await _productRepo.RemoveAsync(id); // Implement this method in your repository
                _logger.LogInformation("Product with ID {ProductId} was deleted successfully.", id);

                // Add a success message to TempData
                TempData["SuccessMessage"] = $"Product {product.Name} was deleted successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a product with ID {ProductId}.", id);
                return View("Error"); // Or, consider a more specific error handling approach
            }
        }

    }
}
