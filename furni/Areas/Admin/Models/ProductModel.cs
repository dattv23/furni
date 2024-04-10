using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace furni.Areas.Admin.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; } // Unique identifier for the product.

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(255, ErrorMessage = "Tên sản phẩm phải ít hơn hoặc bằng 255 ký tự.")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } // Product name.

        [Display(Name = "URL Ảnh")]
        public string? ImageUrl { get; set; } // Url Image product

        [Required(ErrorMessage = "Mô tả sản phẩm là bắt buộc.")]
        [Display(Name = "Mô tả sản phẩm")]
        public string Description { get; set; } // Detailed product description.

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(1, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; } // Product price, using decimal for precision.

        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không thể âm.")]
        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; } // Available stock quantity.

        [Required(ErrorMessage = "CategoryId là bắt buộc.")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }
    }
}
