using System.ComponentModel.DataAnnotations;

namespace SeedStore.Admin.Reviews.Models
{
    public class ModerateReviewModel
    {
        [Required]
        [MaxLength(50)]
        public string StatusCode { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ModeratorComment { get; set; }
    }
}
