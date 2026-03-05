namespace seed_store_api.Store.Modules.Products.Models
{
    public class FilterValueItemModel
    {
        public int Id { get; set; }
        public int FeatureId { get; set; }
        public int? HeaderId { get; set; }
        public string Value { get; set; } = string.Empty;
        public string ValueSlug { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
