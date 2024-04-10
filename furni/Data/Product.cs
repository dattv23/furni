using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; // Necessary for ICollection<T>

namespace furni.Data
{
    // Represents a product in the inventory, including its details, pricing, and availability.
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Unique identifier for the product.

        [Required]
        [StringLength(255, ErrorMessage = "The name must be 255 characters or fewer.")]
        public string Name { get; set; } // Product name.

        [Required]
        public string ImageUrl { get; set; } // Url Image product

        [Required]
        public string Description { get; set; } // Detailed product description.

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; } // Product price, using decimal for precision.

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock Quantity cannot be negative.")]
        public int StockQuantity { get; set; } // Available stock quantity.

        // IsDeleted is a property that holds the product's status deleted product.
        public bool? IsDeleted { get; set; } = false;

        // Category relationship
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } // Link to the product's category.

        public virtual Specification Specification { get; set; } // Optional product specification.

        // Collections for relationships with OrderItem and CartItem, enabling navigation.
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();
    }
}
