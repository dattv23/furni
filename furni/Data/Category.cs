using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace furni.Data
{
    // Represents a product category within the store.
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } // Category name, up to 255 characters.

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; } // Optional longer description of the category.

        // Nullable foreign key to enable hierarchical categorization (self-referencing).
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; } // Navigation property for the parent category.

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Timestamp of category creation in UTC.

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; } // Timestamp of the last update in UTC.

        public bool IsDeleted { get; set; } = false; // Indicates if the category has been soft-deleted, default is false.

        // Collection of products that belong to this category.
        public virtual ICollection<Product> Products { get; set; } = new List<Product>(); // Initializes the collection to prevent null references.
    }
}
