using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Authorization.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
