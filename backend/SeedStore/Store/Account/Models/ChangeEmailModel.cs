using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Account.Models
{
    public class ChangeEmailModel
    {
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string NewEmail { get; set; } = string.Empty;
    }
}
