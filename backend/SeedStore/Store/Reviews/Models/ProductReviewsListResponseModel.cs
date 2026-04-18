namespace SeedStore.Store.Reviews.Models
{
    public class ProductReviewsListResponseModel
    {
        public List<ProductReviewsResponseModel> Reviews { get; set; } = [];
        public int TotalCount { get; set; }
    }
}
