using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Authorization.Models
{
    public class VerifyEmailModel
    {
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
