namespace SeedStore.Store.Orders.Models
{
    public class OrderTransactionsModel
    {
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsSuccess { get; set; }
    }
}