namespace seed_store_api.Store.Support.Payment.Models
{
    public class LiqPayCallbackDataModel
    {
        public string OrderId { get; set; } = string.Empty;
        public long PaymentId { get; set; }
        public string LiqPayOrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? ErrCode { get; set; }
        public string? ErrDescription { get; set; }
    }
}
