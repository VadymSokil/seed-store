using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Products.Models
{
    public class AddDiscountModel
    {
        public int GroupId { get; set; }
        public int ProductId { get; set; }
        [Range(0.01, 100)]
        public decimal DiscountPercent { get; set; }
    }
}
