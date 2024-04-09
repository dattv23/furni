using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace furni.Areas.Admin.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; } // Unique identifier for the product.

        [Required]
        [StringLength(255, ErrorMessage = "The name must be 255 characters or fewer.")]
        public string Name { get; set; } // Product name.

        [Required]
        public string? ImageUrl { get; set; } // Url Image product

        [Required]
        [StringLength(1000, ErrorMessage = "The description must be 1000 characters or fewer.")]
        public string Description { get; set; } // Detailed product description.

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; } // Product price, using decimal for precision.

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity cannot be negative.")]
        public int StockQuantity { get; set; } // Available stock quantity.
    }
}
