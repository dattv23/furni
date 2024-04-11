using furni.Data;
using furni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace furni.Controllers
{
    // Trong CartController.cs
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Order()
        {
            var userId = _userManager.GetUserId(User);


            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();

            return View(cartItems);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (cart == null)
            {
                cart = new Cart { UserId = user.Id };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == cart.UserId);
            if (cartItem != null)
            {
                // Sản phẩm đã có trong giỏ, cập nhật số lượng
                cartItem.Quantity += quantity;
            }
            else
            {
                // Sản phẩm chưa có trong giỏ, thêm mới
                cartItem = new CartItem
                {
                    UserId = cart.UserId,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }


        public async Task<IActionResult> Index()
        {
            string currentUserId = User.Identity.Name; // Hoặc một phương thức khác để lấy UserId, tùy vào cách bạn quản lý người dùng

            var cartItems = await _context.CartItems
                .Where(c => c.UserId == currentUserId) // Lọc cart items dựa trên UserId
                .Include(c => c.Product) // Đảm bảo thông tin sản phẩm được nạp cùng
                .ToListAsync();

            return View(cartItems); // Truyền danh sách cartItems vào view
        }
    }

}
