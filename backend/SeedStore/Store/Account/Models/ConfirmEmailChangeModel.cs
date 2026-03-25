using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Account.Models
{
    public class ConfirmEmailChangeModel
    {
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string NewEmail { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
