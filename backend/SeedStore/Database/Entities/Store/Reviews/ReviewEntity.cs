using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Account;

namespace SeedStore.Database.Entities.Store.Reviews
{
    public class ReviewEntity
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public string ProductImageUrlSnapshot { get; set; } = string.Empty;
        public int AccountId { get; set; }
        public int Rating { get; set; }
        public string? Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public string? ModeratorComment { get; set; }
        public int? TakenByEmployeeId { get; set; }

        public AccountEntity? Account { get; set; }
        public EmployeeEntity? TakenByEmployee { get; set; }
    }
}
