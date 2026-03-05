namespace seed_store_api.Database.Entities.Store.Modules.Products
{
    public class ProductFeatureEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
        public int? HeaderId { get; set; }
        public string Value { get; set; } = string.Empty;
        public string ValueSlug { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }

        public FeatureEntity? Feature { get; set; }
        public FeatureHeaderEntity? Header { get; set; }
    }
}
