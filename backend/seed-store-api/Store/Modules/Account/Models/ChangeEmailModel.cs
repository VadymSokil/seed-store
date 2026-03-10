using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Account.Models
{
    public class ChangeEmailModel
    {
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string NewEmail { get; set; } = string.Empty;
    }
}
