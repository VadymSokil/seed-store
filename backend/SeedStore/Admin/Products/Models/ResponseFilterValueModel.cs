namespace SeedStore.Admin.Products.Models
{
    public class ResponseFilterValueModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int FeatureId { get; set; }
        public int? HeaderId { get; set; }
        public string Value { get; set; } = string.Empty;
        public string ValueSlug { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
