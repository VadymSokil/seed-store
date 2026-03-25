using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Products.Models
{
    public class AddDiscountGroupModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
