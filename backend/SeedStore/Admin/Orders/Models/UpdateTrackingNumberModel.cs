using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Orders.Models
{
    public class UpdateTrackingNumberModel
    {
        [Required]
        [MaxLength(100)]
        public string TrackingNumber { get; set; } = string.Empty;
    }
}
