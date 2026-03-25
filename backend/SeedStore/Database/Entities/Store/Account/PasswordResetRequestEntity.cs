namespace SeedStore.Database.Entities.Store.Account
{
    public class PasswordResetRequestEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
