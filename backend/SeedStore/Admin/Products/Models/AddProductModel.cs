using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Products.Models
{
    public class AddProductModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Article { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(400)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public bool IsActive { get; set; }
    }
}
