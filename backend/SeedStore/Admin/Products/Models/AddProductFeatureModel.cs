using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Products.Models
{
    public class AddProductFeatureModel
    {
        public int FeatureId { get; set; }
        public int? HeaderId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Value { get; set; } = string.Empty;

        [Required]
        [MaxLength(400)]
        public string ValueSlug { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
