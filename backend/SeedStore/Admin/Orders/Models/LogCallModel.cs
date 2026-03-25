using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Orders.Models
{
    public class LogCallModel
    {
        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}
