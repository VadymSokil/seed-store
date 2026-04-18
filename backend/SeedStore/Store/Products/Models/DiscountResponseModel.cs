namespace SeedStore.Store.Products.Models
{
    public class DiscountResponseModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
