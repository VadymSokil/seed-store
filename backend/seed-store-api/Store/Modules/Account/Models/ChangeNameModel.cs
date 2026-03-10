using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Account.Models
{
    public class ChangeNameModel
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MiddleName { get; set; }
    }
}
