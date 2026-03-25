namespace SeedStore.Admin.Products.Models
{
    public class ResponseProductModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Article { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<ProductImageModel> Images { get; set; } = [];
        public List<ProductFeatureModel> Features { get; set; } = [];

    }
}
