using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Products.Models
{
    public class AddFeatureHeaderModel
    {
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
