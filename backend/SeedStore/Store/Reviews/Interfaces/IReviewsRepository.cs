using SeedStore.Database.Entities.Store.Account;
using SeedStore.Database.Entities.Store.Products;
using SeedStore.Database.Entities.Store.Reviews;
using SeedStore.Store.Reviews.Models;

namespace SeedStore.Store.Reviews.Interfaces
{
    public interface IReviewsRepository
    {
        Task<(List<(ReviewEntity Review, ReviewReplyEntity? Reply, AccountEntity Account)> items, int totalCount)> GetProductReviewsAsync(int productId, int page, int pageSize, int? accountId);
        Task<AccountReviewsResponseModel> GetAccountReviewsAsync(int accountId, int page, int pageSize);

        Task<bool> ReviewExistsAsync(int accountId, int productId);
        Task AddReviewAsync(ReviewEntity entity);
        Task<ProductEntity?> GetProductByIdAsync(int productId);
        Task<string?> GetProductFirstImageAsync(int productId);

        Task<ReviewEntity?> GetReviewByIdAsync(int reviewId);
        Task UpdateReviewAsync(ReviewEntity entity);

        Task DeleteReviewAsync(ReviewEntity entity);
    }
}
