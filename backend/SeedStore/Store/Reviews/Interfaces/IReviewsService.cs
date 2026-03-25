using SeedStore.Store.Reviews.Models;

namespace SeedStore.Store.Reviews.Interfaces
{
    public interface IReviewsService
    {
        Task<List<ProductReviewsResponseModel>> GetProductReviewsAsync(int productId);
        Task<List<AccountReviewsResponseModel>> GetAccountReviewsAsync(int accountId);
        Task<string> AddReviewAsync(int accountId, AddReviewModel model);
        Task<string> UpdateReviewAsync(int accountId, int reviewId, ChangeReviewModel model);
        Task<string> DeleteReviewAsync(int accountId, int reviewId);
    }
}
