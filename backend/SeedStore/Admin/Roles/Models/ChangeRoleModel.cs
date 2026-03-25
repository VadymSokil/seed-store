using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Roles.Models
{
    public class ChangeRoleModel
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 99)]
        public int Priority { get; set; }
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
