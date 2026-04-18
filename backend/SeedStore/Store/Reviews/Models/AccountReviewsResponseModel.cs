namespace SeedStore.Store.Reviews.Models
{
    public class AccountReviewsResponseModel
    {
        public int TotalCount { get; set; }
        public List<AccountReviewModel> Items { get; set; } = [];
    }
}
