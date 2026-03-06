namespace seed_store_api.Database.Entities.Store.Modules.Account
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
