using furni.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace furni.Areas.Admin.Models
{
    public class OrdersModel
    {
        public int Id { get; set; } // Unique identifier for the order.

        [Required]
        public string UserId { get; set; } // The ID of the user who placed the order.

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; } // The total cost of the order.

        [StringLength(255)]
        public string Status { get; set; }

        public int? CouponId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
