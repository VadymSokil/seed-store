using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Reviews.Models
{
    public class AddReviewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(2000)]
        public string? Text { get; set; }
    }
}
