namespace seed_store_api.Database.Entities.Store.Modules.Orders
{
    public class OrderStatusEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }

        public ICollection<OrderEntity> Orders { get; set; } = [];
    }
}