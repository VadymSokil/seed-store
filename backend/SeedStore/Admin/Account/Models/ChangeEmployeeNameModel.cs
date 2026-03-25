using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Account.Models
{
    public class ChangeEmployeeNameModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
