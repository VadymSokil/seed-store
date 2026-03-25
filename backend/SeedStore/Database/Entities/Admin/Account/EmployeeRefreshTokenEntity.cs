namespace SeedStore.Database.Entities.Admin.Account
{
    public class EmployeeRefreshTokenEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
