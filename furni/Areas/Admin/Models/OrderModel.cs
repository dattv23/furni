using furni.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Areas.Admin.Models
{
    public class OrderModel
    {
        public int Id { get; set; } // Unique identifier for the order.

        [Required(ErrorMessage = "UserId là bắt buộc.")]
        public string UserId { get; set; } // The ID of the user who placed the order.
        public virtual ApplicationUser User { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự.")]
        public string Address { get; set; } // Address of the order

        [Required(ErrorMessage = "Tổng số tiền là bắt buộc.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; } // The total cost of the order.

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow; // Timestamp when the order was placed, in UTC.

        [StringLength(255, ErrorMessage = "Trạng thái không được vượt quá 255 ký tự.")]
        public string Status { get; set; } // The current status of the order

        public int? CouponId { get; set; } // Optional coupon ID applied to the order
        public virtual Coupon? Coupon { get; set; } // Navigation property

        public virtual ICollection<OrderItem> OrderItems { get; set; } // The items that are part of this order.
    }
}
