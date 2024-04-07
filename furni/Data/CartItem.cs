using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Represents an item within a shopping cart.
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        // Establishes a relationship to the Product entity.
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        // Direct association with a user is assumed. Replace with CartId if using a separate Cart entity.
        [Required]
        public string UserId { get; set; }

        // The required quantity of the product, with a minimum value of 1.
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
