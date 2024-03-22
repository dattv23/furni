using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Represents the physical specifications and quality details of a product.
    public class Specification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Unique identifier for the specification.

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Width must be greater than 0")]
        public int Width { get; set; } // Width of the product in specified units.

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Height must be greater than 0")]
        public int Height { get; set; } // Height of the product in specified units.

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Depth must be greater than 0")]
        public int Depth { get; set; } // Depth of the product in specified units.

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Weight must be greater than 0")]
        public int Weight { get; set; } // Weight of the product in specified units.

        public bool QualityChecking { get; set; } // Indicates if the product has passed quality checking.

        [Range(0, int.MaxValue, ErrorMessage = "Freshness duration must not be negative")]
        public int FreshnessDuration { get; set; } // Duration (in days) the product remains fresh, applicable for perishable goods.

        // Link to the product this specification belongs to.
        [ForeignKey("Product")]
        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } // Navigation property to the associated product.
    }
}
