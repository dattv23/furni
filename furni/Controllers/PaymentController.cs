using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using furni.Extensions;
using furni.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace furni.Controllers
{
    public class PaymentController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult StoreProductSession(List<ProductItem> selectedProducts)
        {
            // Lưu danh sách sản phẩm được chọn vào Session
            HttpContext.Session.SetObjectAsJson("SelectedProducts", selectedProducts);

            return Json(new { success = true });
        }
        public IActionResult Payment()
        {
            // Lấy danh sách sản phẩm từ Session
            var selectedProducts = HttpContext.Session.GetObjectFromJson<List<ProductItem>>("SelectedProducts");

            return View(selectedProducts);
        }
    }
}

