using SeedStore.Database.Context;
using SeedStore.Database.Entities.Store.Reviews;
using SeedStore.Store.Products.Interfaces;
using SeedStore.Store.Reviews.Interfaces;
using SeedStore.Store.Reviews.Models;
using SeedStore.Support.General.Constants.Store;
using SeedStore.Support.Store.Notifications.Interfaces;

namespace SeedStore.Store.Reviews.Services
{
    public class ReviewsService : IReviewsService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IReviewsRepository _reviewsRepository;
        private readonly INotificationService _notificationService;
        private readonly IProductRepository _productRepository;

        public ReviewsService(AppDbContext appDbContext, IReviewsRepository reviewsRepository, INotificationService notificationService, IProductRepository productRepository)
        {
            _appDbContext = appDbContext;
            _reviewsRepository = reviewsRepository;
            _notificationService = notificationService;
            _productRepository = productRepository;
        }

        public async Task<List<ProductReviewsResponseModel>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _reviewsRepository.GetProductReviewsAsync(productId);

            return reviews.Select(x => new ProductReviewsResponseModel
            {
                Id = x.Item1.Id,
                AccountId = x.Item1.AccountId,
                FirstName = x.Item3.FirstName,
                LastName = x.Item3.LastName,
                Rating = x.Item1.Rating,
                Text = x.Item1.Text,
                CreatedAt = x.Item1.CreatedAt,
                UpdatedAt = x.Item1.UpdatedAt,
                Reply = x.Item2?.Text,
                ReplyCreatedAt = x.Item2?.CreatedAt,
                ReplyUpdatedAt = x.Item2?.UpdatedAt
            }).ToList();
        }

        public async Task<List<AccountReviewsResponseModel>> GetAccountReviewsAsync(int accountId)
        {
            var reviews = await _reviewsRepository.GetAccountReviewsAsync(accountId);

            return reviews.Select(x => new AccountReviewsResponseModel
            {
                Id = x.Item1.Id,
                ProductId = x.Item1.ProductId,
                ProductNameSnapshot = x.Item1.ProductNameSnapshot,
                ProductImageUrlSnapshot = x.Item1.ProductImageUrlSnapshot,
                Rating = x.Item1.Rating,
                Text = x.Item1.Text,
                CreatedAt = x.Item1.CreatedAt,
                UpdatedAt = x.Item1.UpdatedAt,
                StatusCode = x.Item1.StatusCode,
                ModeratorComment = x.Item1.ModeratorComment,
                Reply = x.Item2?.Text,
                ReplyCreatedAt = x.Item2?.CreatedAt,
                ReplyUpdatedAt = x.Item2?.UpdatedAt
            }).ToList();
        }

        public async Task<string> AddReviewAsync(int accountId, AddReviewModel model)
        {
            var product = await _reviewsRepository.GetProductByIdAsync(model.ProductId);

            if (product == null)
                return "product_not_found";

            var reviewExists = await _reviewsRepository.ReviewExistsAsync(accountId, model.ProductId);

            if (reviewExists)
                return "review_exists";

            var productImage = await _reviewsRepository.GetProductFirstImageAsync(model.ProductId);

            var entity = new ReviewEntity
            {
                ProductId = model.ProductId,
                ProductNameSnapshot = product.Name,
                ProductImageUrlSnapshot = productImage ?? string.Empty,
                AccountId = accountId,
                Rating = model.Rating,
                Text = model.Text,
                CreatedAt = DateTime.UtcNow,
                StatusCode = ReviewStatusCodes.Pending
            };

            await _reviewsRepository.AddReviewAsync(entity);
            await _productRepository.RecalculateProductRatingAsync(model.ProductId);
            await _notificationService.SendNewReviewAsync();

            return "ok";
        }

        public async Task<string> UpdateReviewAsync(int accountId, int reviewId, ChangeReviewModel model)
        {
            var review = await _reviewsRepository.GetReviewByIdAsync(reviewId);

            if (review == null)
                return "not_found";

            if (review.AccountId != accountId)
                return "forbidden";

            review.Rating = model.Rating;
            review.Text = model.Text;
            review.UpdatedAt = DateTime.UtcNow;
            review.StatusCode = ReviewStatusCodes.Pending;

            await _reviewsRepository.UpdateReviewAsync(review);
            await _productRepository.RecalculateProductRatingAsync(review.ProductId!.Value);
            await _notificationService.SendNewReviewAsync();

            return "ok";
        }

        public async Task<string> DeleteReviewAsync(int accountId, int reviewId)
        {
            var review = await _reviewsRepository.GetReviewByIdAsync(reviewId);

            if (review == null)
                return "not_found";

            if (review.AccountId != accountId)
                return "forbidden";

            await _reviewsRepository.DeleteReviewAsync(review);
            await _productRepository.RecalculateProductRatingAsync(review.ProductId!.Value);

            return "ok";
        }
    }
}
