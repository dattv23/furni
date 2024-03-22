using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Represents a shopping cart associated with a user.
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign key for ApplicationUser
        public string UserId { get; set; }

        // Navigation property for the cart's user.
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        // Collection of items within the cart.
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        // Constructor is not needed if only used to initialize navigation properties
        // as this is handled by the property initializer above.
    }
}
