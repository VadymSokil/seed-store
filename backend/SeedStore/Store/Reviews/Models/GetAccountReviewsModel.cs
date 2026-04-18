using System.ComponentModel.DataAnnotations;

namespace SeedStore.Store.Reviews.Models
{
    public class GetAccountReviewsModel
    {
        public int Page { get; set; }
        [Range(1, 50)]
        public int PageSize { get; set; }
    }
}
