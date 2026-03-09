namespace seed_store_api.Store.Modules.Orders.Models
{
    public class OrderTransactionsModel
    {
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSuccess { get; set; }
    }
}