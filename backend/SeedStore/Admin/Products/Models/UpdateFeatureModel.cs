using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Products.Models
{
    public class UpdateFeatureModel
    {
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
