namespace seed_store_api.Store.Modules.Products.Models
{
    public class ProductFeatureItemModel
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; } = string.Empty;
        public string FeatureSlug { get; set; } = string.Empty;
        public int? HeaderId { get; set; }
        public string? HeaderName { get; set; }
        public string Value { get; set; } = string.Empty;
        public string ValueSlug { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
