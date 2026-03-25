using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Reviews;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Reviews.Interfaces
{
    public interface IAdminReviewsRepository
    {
        Task<List<ReviewEntity>> GetReviewsAsync(string? statusCode);
        Task<ReviewEntity?> GetReviewByIdAsync(int reviewId);
        Task UpdateReviewAsync(ReviewEntity review);
        Task<ReviewReplyEntity?> GetReplyByReviewIdAsync(int reviewId);
        Task<ReviewReplyEntity?> GetReplyByIdAsync(int replyId);
        Task AddReplyAsync(ReviewReplyEntity reply);
        Task UpdateReplyAsync(ReviewReplyEntity reply);
        Task DeleteReplyAsync(ReviewReplyEntity reply);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);

        Task<List<ReviewStatusEntity>> GetReviewStatusesAsync();
        Task<ReviewStatusEntity?> GetReviewStatusByIdAsync(int statusId);
        Task<bool> ReviewStatusCodeExistsAsync(string code);
        Task AddReviewStatusAsync(ReviewStatusEntity status);
        Task UpdateReviewStatusAsync(ReviewStatusEntity status);
        Task DeleteReviewStatusAsync(ReviewStatusEntity status);

        Task ReorderReviewStatusesAsync(List<ReorderItemModel> items);

        Task<List<ReorderItemModel>> GetReviewStatusesOrderAsync();
    }
}
