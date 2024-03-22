using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Represents an individual item within an order, including its product, quantity, and pricing information.
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; } // Links to the Order that includes this item.

        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } // Links to the Product being ordered.

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; } // The price per unit at the time of the order.

        [Required]
        public int Quantity { get; set; } // The number of units of the product ordered.

        // This property calculates the total cost for this item, applying the discount.
        // It is not mapped to the database.
        [NotMapped]
        public decimal Total => (Price * Quantity); // Total cost after discount.
    }
}
