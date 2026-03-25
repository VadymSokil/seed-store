using SeedStore.Admin.Reviews.Models;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Reviews.Interfaces
{
    public interface IAdminReviewsService
    {
        Task<List<ResponseReviewsModel>> GetReviewsAsync(string? statusCode);
        Task<ResponseReviewsModel?> GetReviewAsync(int reviewId);
        Task<string> ModerateReviewAsync(int reviewId, ModerateReviewModel model, int initianorId);
        Task<string> AddReplyAsync(int reviewId, string text, int initianorId);
        Task<string> UpdateReplyAsync(int replyId, string text, int initianorId);
        Task<string> DeleteReplyAsync(int replyId, int initianorId);

        Task<List<ResponseReviewStatusesModel>> GetReviewStatusesAsync();
        Task<string> AddReviewStatusAsync(AddReviewStatusModel model, int initiatorId);
        Task<string> UpdateReviewStatusAsync(int statusId, UpdateReviewStatusModel model, int initiatorId);
        Task<string> DeleteReviewStatusAsync(int statusId, int initiatorId);

        Task ReorderReviewStatusesAsync(List<ReorderItemModel> items, int initiatorId);

        Task<string> TakeReviewAsync(int reviewId, int employeeId);
        Task<string> ReleaseReviewAsync(int reviewId, int employeeId);
        Task<string> ResetReviewAsync(int reviewId, int initiatorId);
    }
}
