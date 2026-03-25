namespace SeedStore.Database.Entities.Store.Account
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
