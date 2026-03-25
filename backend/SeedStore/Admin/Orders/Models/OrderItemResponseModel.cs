namespace SeedStore.Admin.Orders.Models
{
    public class OrderItemResponseModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public string? ProductImageUrlSnapshot { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
