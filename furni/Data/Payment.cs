using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Defines a payment transaction associated with an order.
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Unique identifier for the payment.

        [Required]
        public int OrderId { get; set; } // Reference to the associated order.
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; } // Navigation property for the related order.

        [Required]
        public int PaymentMethodId { get; set; } // Identifies the payment method used.
        [ForeignKey(nameof(PaymentMethodId))]
        public virtual PaymentMethod PaymentMethod { get; set; } // Navigation property for the payment method details.

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; } // The amount paid in this transaction.

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow; // The date and time when the payment was processed.

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // The status of the payment (e.g., Pending, Completed, Failed).
    }
}
