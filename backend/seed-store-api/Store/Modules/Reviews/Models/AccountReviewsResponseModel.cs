namespace seed_store_api.Store.Modules.Reviews.Models
{
    public class AccountReviewsResponseModel
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public string ProductImageUrlSnapshot { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; }
        public string? ModeratorComment { get; set; }
        public string? Reply {  get; set; }
        public DateTime? ReplyCreatedAt { get; set; }
        public DateTime? ReplyUpdatedAt { get; set; }

    }
}
