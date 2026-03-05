namespace seed_store_api.Database.Entities.Store.Modules.StoreInfo
{
    public class PaymentVariantEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
