namespace SeedStore.Database.Entities.Store.Reviews
{
    public class ReviewReplyEntity
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ReviewEntity? Review { get; set; }

    }
}
