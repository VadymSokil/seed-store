using SeedStore.Database.Entities.Store.Account;
using SeedStore.Database.Entities.Store.Products;
using SeedStore.Database.Entities.Store.Reviews;

namespace SeedStore.Store.Reviews.Interfaces
{
    public interface IReviewsRepository
    {
        Task<List<(ReviewEntity Review, ReviewReplyEntity? Reply, AccountEntity Account)>> GetProductReviewsAsync(int productId);
        Task<List<(ReviewEntity Review, ReviewReplyEntity? Reply)>> GetAccountReviewsAsync(int accountId);

        Task<bool> ReviewExistsAsync(int accountId, int productId);
        Task AddReviewAsync(ReviewEntity entity);
        Task<ProductEntity?> GetProductByIdAsync(int productId);
        Task<string?> GetProductFirstImageAsync(int productId);

        Task<ReviewEntity?> GetReviewByIdAsync(int reviewId);
        Task UpdateReviewAsync(ReviewEntity entity);

        Task DeleteReviewAsync(ReviewEntity entity);
    }
}
