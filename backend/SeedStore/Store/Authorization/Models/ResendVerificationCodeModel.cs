using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Authorization.Models
{
    public class ResendVerificationCodeModel
    {
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
