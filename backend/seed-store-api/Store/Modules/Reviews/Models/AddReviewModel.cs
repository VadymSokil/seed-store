using System.ComponentModel.DataAnnotations;

namespace seed_store_api.Store.Modules.Reviews.Models
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
