using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Represents a customer's order, including order details, user information, and associated coupon.
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Unique identifier for the order.

        [Required]
        public string UserId { get; set; } // The ID of the user who placed the order.
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } // Navigation property linking to the user who owns this order.

        [StringLength(500)]
        public string Address { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Timestamp when the order was placed, in UTC.

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; } // The total cost of the order.

        [StringLength(255)]
        public string Status { get; set; } // The current status of the order (e.g., Pending, Shipped, Delivered, Cancelled).

        public bool IsDeleted { get; set; } = false; // Indicates whether the order has been marked as deleted (soft delete).
        public DateTime? DeletedAt { get; set; } // Timestamp for when the order was soft deleted, if applicable.

        // Navigation properties for related entities
        public virtual ICollection<OrderItem> OrderItems { get; set; } // The items that are part of this order.

        // New properties for the Coupon relationship
        public int? CouponId { get; set; } // Nullable to represent 0..1 relationship
        public virtual Coupon? Coupon { get; set; } // Navigation property

    }
}
