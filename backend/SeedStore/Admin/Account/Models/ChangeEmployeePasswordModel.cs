using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Account.Models
{
    public class ChangeEmployeePasswordModel
    {
        [Required]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
