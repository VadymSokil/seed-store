using SeedStore.Store.Reviews.Models;

namespace SeedStore.Store.Reviews.Interfaces
{
    public interface IReviewsService
    {
        Task<ProductReviewsListResponseModel> GetProductReviewsAsync(int productId, int page, int pageSize, int? accountId);
        Task<AccountReviewsResponseModel> GetAccountReviewsAsync(int accountId, int page, int pageSize);
        Task<string> AddReviewAsync(int accountId, AddReviewModel model);
        Task<string> UpdateReviewAsync(int accountId, int reviewId, ChangeReviewModel model);
        Task<string> DeleteReviewAsync(int accountId, int reviewId);
    }
}
