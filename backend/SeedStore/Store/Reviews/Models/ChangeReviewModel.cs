using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Reviews.Models
{
    public class ChangeReviewModel
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string? Text { get; set; }
    }
}
