using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Account.Models
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
