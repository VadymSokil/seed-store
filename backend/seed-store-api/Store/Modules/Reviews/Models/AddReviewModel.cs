namespace seed_store_api.Store.Modules.Reviews.Models
{
    public class AddReviewModel
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string? Text { get; set; }
    }
}
