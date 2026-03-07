namespace seed_store_api.Store.Modules.Account.Models
{
    public class ConfirmEmailChangeModel
    {
        public string NewEmail { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
