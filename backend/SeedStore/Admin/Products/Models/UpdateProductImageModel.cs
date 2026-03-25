using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Products.Models
{
    public class UpdateProductImageModel
    {
        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
