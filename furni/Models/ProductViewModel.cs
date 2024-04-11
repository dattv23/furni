using System.Collections.Generic;
using furni.Data; // Sử dụng namespace chứa định nghĩa của model Product

namespace furni.Models
{
    public class ProductViewModel
    {
        public List<Product>? Products { get; set; } // Danh sách sản phẩm để hiển thị
        public List<CategoryViewModel> ChildCategories { get; set; }
        public int PageNumber { get; set; } // Số trang hiện tại
        public int TotalPages { get; set; } // Tổng số trang
        public int TotalProduct { get; set; } // Tổng số sản phẩm hiển thị
        // Phương thức này được sử dụng để kiểm tra xem có cần hiển thị nút 'Next' và 'Previous' không
        public bool ShowPrevious => PageNumber > 1;
        public bool ShowNext => PageNumber < TotalPages;
        public List<CategoryViewModel> ParentCategory { get; set; }
        public string SortBy { get; set; }
    }

}