using seed_store_api.Database.Entities.Store.Modules.Products;

namespace seed_store_api.Database.Entities.Store.Modules.Orders
{
    public class OrderItemEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public string? ProductImageUrlSnapshot { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public OrderEntity? Order { get; set; }
        public ProductEntity? Product { get; set; }
    }
}