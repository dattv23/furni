using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Defines a product image, including its file path, alternative text, and association with a product.
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Unique identifier for the product image.

        [Required]
        public string ImageUrl { get; set; } // URL or relative path to the image file.

        [StringLength(255)]
        public string AltText { get; set; } // Alternative text for the image, improving accessibility and SEO.

        // ProductImage relationships
        // Foreign key and navigation property to associate the image with a product.
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
