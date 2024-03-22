using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace furni.Data
{
    // ApplicationUser extends IdentityUser, integrating with ASP.NET Core Identity.
    // It includes additional properties for the user's first and last names and defines 
    // a one-to-one relationship with Cart and a one-to-many relationship with Orders.
    public class ApplicationUser : IdentityUser
    {
        // FirstName is a required property that holds the user's first name.
        // It is initialized to a non-null value to satisfy the compiler's nullability checks.
        public string FirstName { get; set; } = null!;

        // LastName is a required property that holds the user's last name.
        // It is initialized to a non-null value to satisfy the compiler's nullability checks.
        public string LastName { get; set; } = null!;

        // Navigation property for the one-to-many relationship with Orders.
        // A user can place multiple orders.
        public virtual ICollection<Order> Orders { get; set; }

        // Navigation property for the one-to-one relationship with Cart.
        // The '?' indicates that the user may not always have a cart associated with them.
        public Cart Cart { get; set; }

        // The constructor initializes the Orders collection to ensure that it is never null,
        // thus preventing potential null reference exceptions when accessing the user's orders.
        public ApplicationUser()
        {
            Orders = new HashSet<Order>();
        }
    }
}
