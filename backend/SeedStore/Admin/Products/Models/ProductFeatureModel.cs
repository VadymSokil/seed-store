namespace SeedStore.Admin.Products.Models
{
    public class ProductFeatureModel
    {
        public int Id { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; } = string.Empty;
        public int? HeaderId { get; set; }
        public string? HeaderName { get; set; }
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
