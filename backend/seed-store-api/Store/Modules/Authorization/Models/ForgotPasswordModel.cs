using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Authorization.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
