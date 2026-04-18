namespace SeedStore.Store.Orders.Models
{
    public class AccountOrdersResponseModel
    {
        public int TotalCount { get; set; }
        public List<AccountOrderModel> Items { get; set; } = [];
    }
}
