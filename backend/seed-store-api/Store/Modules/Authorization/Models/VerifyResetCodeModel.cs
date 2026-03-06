namespace seed_store_api.Store.Modules.Authorization.Models
{
    public class VerifyResetCodeModel
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
