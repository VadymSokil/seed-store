namespace seed_store_api.Store.Modules.Products.Models
{
    public class FeatureItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
