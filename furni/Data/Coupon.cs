using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Defines the characteristics of a discount coupon in the system.
    public class Coupon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } // Unique identifier for the coupon.

        [Required]
        [Range(0, 100)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountPercentage { get; set; } // Percentage discount the coupon provides.

        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; } // Start date from when the coupon can be used.

        [DataType(DataType.Date)]
        public DateTime ValidUntil { get; set; } // Expiry date after which the coupon cannot be used.

        public bool IsActive { get; set; } = true; // Indicates whether the coupon is currently usable.

        // Minimum order amount required for the coupon to be applicable, if any.
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MinimumOrderAmount { get; set; }
    }
}
