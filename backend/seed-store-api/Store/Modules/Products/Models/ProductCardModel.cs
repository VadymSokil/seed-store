namespace seed_store_api.Store.Modules.Products.Models
{
    public class ProductCardModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Article { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool HasDiscount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public int Quantity { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public string ImageUrl { get; set; }
    }
}
