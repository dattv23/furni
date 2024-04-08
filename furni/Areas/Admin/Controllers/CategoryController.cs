//using furni.Data;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;

//namespace furni.Areas.Admin.Controllers
//{
//    [Authorize(Roles = "admin")]
//    [Area("Admin")]
//    public class CategoryController : Controller
//    {
       
//        private readonly ApplicationDbContext _context;
//        public CategoryController(ApplicationDbContext context )
//        {
//            _context = context;
           
//        }
//        public IActionResult Index()
//        {
//            var category = _context.Categories.ToList();
//            return View(category);
//        }

//        public IActionResult Create()
//        {
//            var categories = _context.Categories
//            .Where(c => c.Id >= 1 && c.Id <= 4)
//            .Select(c => new SelectListItem
//            {
//                 Value = c.Id.ToString(), 
//                Text = c.Name
//            })
//                .ToList();
//            ViewBag.Categories = categories;


//            return View();
//        }


//        [HttpPost]
//        public async Task<IActionResult> Create([FromForm] Category category)
//        {
//            // Kiểm tra xem dữ liệu đầu vào có hợp lệ không
//            if (!ModelState.IsValid)
//            {
//                // Lấy thông tin lỗi từ ModelState và trả về cho client
//                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
//                return BadRequest(new { success = false, message = errors });
//            }

//            // Nếu dữ liệu hợp lệ, tiếp tục xử lý
//            _context.Categories.Add(category);
//            await _context.SaveChangesAsync();

//            // Trả về phản hồi thành công
//            return Ok(new { success = true, message = "Category added successfully" });
//        }
//        public IActionResult Update()
//        {
//            var categories = _context.Categories
//           .Where(c => c.Id >= 1 && c.Id <= 4)
//           .Select(c => new SelectListItem
//           {
//               Value = c.Id.ToString(),
//               Text = c.Name
//           })
//               .ToList();
//            ViewBag.Categories = categories;


//            return View();
//        }

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Delete(int id)
//        //{
//        //    var category = await _context.Categories.FindAsync(id);
//        //    if (category == null)
//        //    {
//        //        return NotFound();
//        //    }

//        //    // Đánh dấu danh mục là đã bị xóa mềm
//        //    category.IsDeleted = true;

//        //    // Lưu thay đổi vào cơ sở dữ liệu
//        //    await _context.SaveChangesAsync();

//        //    return Ok(new { success = true, message = "Danh mục đã được xóa mềm thành công." });
//        //}

//        //[ValidateAntiForgeryToken]
//        //[HttpPost("DeleteSelected")]
//        //public async Task<IActionResult> DeleteSelected([FromBody] List<int> ids)
//        //{
//        //    var categories = await _context.Categories.Where(c => ids.Contains(c.Id)).ToListAsync();
//        //    foreach (var category in categories)
//        //    {
//        //        category.IsDeleted = true; // Đánh dấu xóa mềm
//        //    }
//        //    await _context.SaveChangesAsync();
//        //    return Ok(new { success = true, message = "Các danh mục đã được xóa mềm thành công." });
//        //}
//        public IActionResult Delete()
//        {
//            var category = _context.Categories.ToList();
//            return View(category);
//        }
//    }
//}
