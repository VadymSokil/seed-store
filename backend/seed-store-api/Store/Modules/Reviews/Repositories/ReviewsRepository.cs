using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Context;
using seed_store_api.Database.Entities.Store.Modules.Account;
using seed_store_api.Database.Entities.Store.Modules.Products;
using seed_store_api.Database.Entities.Store.Modules.Reviews;
using seed_store_api.Store.Modules.Reviews.Interfaces;

namespace seed_store_api.Store.Modules.Reviews.Repositories
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly AppDbContext _context;
        
        public ReviewsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<(ReviewEntity Review, ReviewReplyEntity? Reply, AccountEntity Account)>> GetProductReviewsAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId && r.IsApproved)
                .Join(
                    _context.Accounts,
                    r => r.AccountId,
                    a => a.Id,
                    (r, a) => new { Review = r, Account = a })
                .GroupJoin(
                    _context.ReviewReplies,
                    x => x.Review.Id,
                    rr => rr.ReviewId,
                    (x, replies) => new { x.Review, x.Account, Replies = replies })
                .SelectMany(
                    x => x.Replies.DefaultIfEmpty(),
                    (x, reply) => new ValueTuple<ReviewEntity, ReviewReplyEntity?, AccountEntity>(x.Review, reply, x.Account))
                .ToListAsync();
        }

        public async Task<List<(ReviewEntity Review, ReviewReplyEntity? Reply)>> GetAccountReviewsAsync(int accountId)
        {
            return await _context.Reviews
                .Where(r => r.AccountId == accountId)
                .GroupJoin(
                    _context.ReviewReplies,
                    r => r.Id,
                    rr => rr.ReviewId,
                    (r, replies) => new { Review = r, Replies = replies })
                .SelectMany(
                    x => x.Replies.DefaultIfEmpty(),
                    (x, reply) => new ValueTuple<ReviewEntity, ReviewReplyEntity?>(x.Review, reply))
                .ToListAsync();
        }



        public async Task<bool> ReviewExistsAsync(int accountId, int productId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.AccountId == accountId && r.ProductId == productId);
        }

        public async Task AddReviewAsync(ReviewEntity entity)
        {
            await _context.Reviews.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<ProductEntity?> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<string?> GetProductFirstImageAsync(int productId)
        {
            return await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .OrderBy(pi => pi.ViewOrder)
                .Select(pi => pi.Url)
                .FirstOrDefaultAsync();
        }



        public async Task<ReviewEntity?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task UpdateReviewAsync(ReviewEntity entity)
        {
            _context.Reviews.Update(entity);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteReviewAsync(ReviewEntity entity)
        {
            _context.Reviews.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
