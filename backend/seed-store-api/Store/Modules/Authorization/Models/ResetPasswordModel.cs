using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Authorization.Models
{
    public class ResetPasswordModel
    {
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
