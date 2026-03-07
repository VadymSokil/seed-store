namespace seed_store_api.Store.Modules.Account.Models
{
    public class ChangeNameModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
    }
}
