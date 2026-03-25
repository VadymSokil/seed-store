using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Roles.Models
{
    public class AddPermissionModel
    {
        [Required]
        [MaxLength(100)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
