using seed_store_api.Database.Entities.Store.Modules.Account;
using seed_store_api.Database.Entities.Store.Modules.Products;
using seed_store_api.Database.Entities.Store.Modules.Reviews;

namespace seed_store_api.Store.Modules.Reviews.Interfaces
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
