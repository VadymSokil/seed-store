namespace SeedStore.Admin.Reviews.Models
{
    public class ResponseReviewsModel
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public string ProductImageUrlSnapshot { get; set; } = string.Empty;
        public int AccountId { get; set; }
        public int Rating { get; set; }
        public string? Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public string? ModeratorComment { get; set; } = string.Empty;


    }
}
