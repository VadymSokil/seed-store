namespace seed_store_api.Store.Modules.Reviews.Models
{
    public class ProductReviewsResponseModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get;set; }
        public string? Reply {  get; set; }
        public DateTime? ReplyCreatedAt { get; set; }
        public DateTime? ReplyUpdatedAt { get; set; }
    }
}
