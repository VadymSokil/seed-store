using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Reviews.Models
{
    public class GetProductReviewsModel
    {
        [Required]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue)]
        public int Page { get; set; }
        [Range(1, 50)]
        public int PageSize { get; set; } 
    }
}
