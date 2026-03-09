namespace seed_store_api.Store.Modules.Orders.Models
{
    public class OrderItemsModel
    {
        public int ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public string? ProductImageUrlSnapshot { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}