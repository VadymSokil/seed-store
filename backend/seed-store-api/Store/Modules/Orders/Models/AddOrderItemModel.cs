namespace seed_store_api.Store.Modules.Orders.Models
{
    public class AddOrderItemModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
