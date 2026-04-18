using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Authorization.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
