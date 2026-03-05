namespace seed_store_api.Store.Modules.Products.Models
{
    public class FeatureHeaderItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
