namespace seed_store_api.Database.Entities.Store.Modules.Products
{
    public class FeatureHeaderEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ViewOrder { get; set; }
    }
}
