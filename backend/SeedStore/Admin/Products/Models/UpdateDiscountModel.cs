using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Products.Models
{
    public class UpdateDiscountModel
    {
        [Range(0.01, 100)]
        public decimal DiscountPercent { get; set; }
    }
}
