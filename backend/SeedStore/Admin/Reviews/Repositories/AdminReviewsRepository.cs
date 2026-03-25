using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.Reviews.Interfaces;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Reviews;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Reviews.Repositories
{
    public class AdminReviewsRepository : IAdminReviewsRepository
    {
        private readonly AppDbContext _context;

        public AdminReviewsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewEntity>> GetReviewsAsync(string? statusCode)
        {
            var query = _context.Reviews.AsQueryable();

            if (!string.IsNullOrEmpty(statusCode))
                query = query.Where(r => r.StatusCode == statusCode);

            return await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
        }

        public async Task<ReviewEntity?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .Include(r => r.Account)
                .FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task UpdateReviewAsync(ReviewEntity review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task<ReviewReplyEntity?> GetReplyByReviewIdAsync(int reviewId)
        {
            return await _context.ReviewReplies.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<ReviewReplyEntity?> GetReplyByIdAsync(int replyId)
        {
            return await _context.ReviewReplies
                .Include(r => r.Review)
                    .ThenInclude(r => r!.Account)
                .FirstOrDefaultAsync(r => r.Id == replyId);
        }

        public async Task AddReplyAsync(ReviewReplyEntity reply)
        {
            await _context.ReviewReplies.AddAsync(reply);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReplyAsync(ReviewReplyEntity reply)
        {
            _context.ReviewReplies.Update(reply);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReplyAsync(ReviewReplyEntity reply)
        {
            _context.ReviewReplies.Remove(reply);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<List<ReviewStatusEntity>> GetReviewStatusesAsync()
        {
            return await _context.ReviewStatuses.OrderBy(s => s.ViewOrder).ToListAsync();
        }

        public async Task<ReviewStatusEntity?> GetReviewStatusByIdAsync(int statusId)
        {
            return await _context.ReviewStatuses.FirstOrDefaultAsync(s => s.Id == statusId);
        }

        public async Task<bool> ReviewStatusCodeExistsAsync(string code)
        {
            return await _context.ReviewStatuses.AnyAsync(s => s.Code == code);
        }

        public async Task AddReviewStatusAsync(ReviewStatusEntity status)
        {
            status.ViewOrder = await GetNextReviewStatusViewOrderAsync();
            await _context.ReviewStatuses.AddAsync(status);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewStatusAsync(ReviewStatusEntity status)
        {
            _context.ReviewStatuses.Update(status);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewStatusAsync(ReviewStatusEntity status)
        {
            _context.ReviewStatuses.Remove(status);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetNextReviewStatusViewOrderAsync()
        {
            return (await _context.ReviewStatuses.MaxAsync(c => c.ViewOrder) ?? 0) + 1;
        }

        public async Task ReorderReviewStatusesAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.ReviewStatuses
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task<List<ReorderItemModel>> GetReviewStatusesOrderAsync()
        {
            return await _context.ReviewStatuses
                .OrderBy(s => s.ViewOrder)
                .Select(s => new ReorderItemModel { Id = s.Id, ViewOrder = s.ViewOrder ?? 0 })
                .ToListAsync();
        }
    }
}