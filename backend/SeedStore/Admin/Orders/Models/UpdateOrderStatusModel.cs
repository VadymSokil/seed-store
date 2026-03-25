using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Orders.Models
{
    public class UpdateOrderStatusModel
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ViewOrder { get; set; }
    }
}
