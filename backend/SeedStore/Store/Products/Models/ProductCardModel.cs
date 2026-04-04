namespace SeedStore.Store.Products.Models
{
    public class ProductCardModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool HasDiscount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public string ImageUrl { get; set; }
    }
}
