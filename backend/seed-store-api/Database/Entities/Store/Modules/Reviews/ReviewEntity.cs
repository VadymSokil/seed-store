namespace seed_store_api.Database.Entities.Store.Modules.Reviews
{
    public class ReviewEntity
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public string ProductImageUrlSnapshot {  get; set; } = string.Empty;
        public int AccountId { get; set; }
        public int Rating { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; }
        public string? ModeratorComment { get; set; }
    }
}
