namespace seed_store_api.Database.Entities.Store.Modules.Orders
{
    public class OrderTransactionEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public long LiqPayPaymentId { get; set; }
        public string LiqPayOrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? ErrCode { get; set; }
        public string? ErrDescription { get; set; }
        public DateTime CreatedAt { get; set; }

        public OrderEntity? Order { get; set; }
    }
}