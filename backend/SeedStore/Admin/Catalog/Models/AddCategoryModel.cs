using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Catalog.Models
{
    public class AddCategoryModel
    {
        public int? ParentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
