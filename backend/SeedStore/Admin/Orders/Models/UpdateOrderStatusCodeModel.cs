using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Orders.Models
{
    public class UpdateOrderStatusCodeModel
    {
        [Required]
        [MaxLength(50)]
        public string StatusCode { get; set; } = string.Empty;
    }
}
