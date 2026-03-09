namespace seed_store_api.Store.Modules.Orders.Models
{
    public class PaymentCallbackModel
    {
        public string Data { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
