namespace seed_store_api.Database.Entities.Store.Modules.Products
{
    public class FeatureEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
