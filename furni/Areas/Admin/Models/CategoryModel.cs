using furni.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace furni.Areas.Admin.Models
{
    public class CategoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } // Category name, up to 255 characters.

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; } // Optional longer description of the category.

        // Nullable foreign key to enable hierarchical categorization (self-referencing).
        public int? ParentId { get; set; }

    }
}
