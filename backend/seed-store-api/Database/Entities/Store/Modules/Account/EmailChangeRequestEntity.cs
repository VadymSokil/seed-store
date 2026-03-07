namespace seed_store_api.Database.Entities.Store.Modules.Account
{
    public class EmailChangeRequestEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string NewEmail { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
