using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Account.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
