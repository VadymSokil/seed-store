using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Account.Models
{
    public class BlockEmployeeModel
    {
        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }
}
