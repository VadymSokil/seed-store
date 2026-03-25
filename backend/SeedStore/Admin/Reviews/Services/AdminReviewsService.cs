using SeedStore.Admin.Reviews.Interfaces;
using SeedStore.Admin.Reviews.Models;
using SeedStore.Database.Entities.Store.Reviews;
using SeedStore.Store.Products.Interfaces;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.Admin.Reorder.Models;
using SeedStore.Support.General.Constants.Store;
using SeedStore.Support.Store.Notifications.Interfaces;
using System.Text.Json;

namespace SeedStore.Admin.Reviews.Services
{
    public class AdminReviewsService : IAdminReviewsService
    {
        private readonly IAdminReviewsRepository _repository;
        private readonly IEmployeesActivityService _employeesActivityService;
        private readonly IProductRepository _productRepository;
        private readonly INotificationService _notificationService;

        public AdminReviewsService(IAdminReviewsRepository repository, IEmployeesActivityService employeesActivityService, IProductRepository productRepository, INotificationService notificationService)
        {
            _repository = repository;
            _employeesActivityService = employeesActivityService;
            _productRepository = productRepository;
            _notificationService = notificationService;
        }

        public async Task<List<ResponseReviewsModel>> GetReviewsAsync(string? statusCode)
        {
            var reviews = await _repository.GetReviewsAsync(statusCode);

            return reviews.Select(r => new ResponseReviewsModel
            {
                Id = r.Id,
                ProductId = r.ProductId,
                ProductNameSnapshot = r.ProductNameSnapshot,
                ProductImageUrlSnapshot = r.ProductImageUrlSnapshot,
                AccountId = r.AccountId,
                Rating = r.Rating,
                Text = r.Text,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                StatusCode = r.StatusCode,
                ModeratorComment = r.ModeratorComment
            }).ToList();
        }

        public async Task<ResponseReviewsModel?> GetReviewAsync(int reviewId)
        {
            var review = await _repository.GetReviewByIdAsync(reviewId);

            if (review == null)
                return null;

            return new ResponseReviewsModel
            {
                Id = review.Id,
                ProductId = review.ProductId,
                ProductNameSnapshot = review.ProductNameSnapshot,
                ProductImageUrlSnapshot = review.ProductImageUrlSnapshot,
                AccountId = review.AccountId,
                Rating = review.Rating,
                Text = review.Text,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt,
                StatusCode = review.StatusCode,
                ModeratorComment = review.ModeratorComment
            };
        }

        public async Task<string> ModerateReviewAsync(int reviewId, ModerateReviewModel model, int initiatorId)
        {
            var review = await _repository.GetReviewByIdAsync(reviewId);

            if (review == null)
                return "not_found";

            var before = JsonSerializer.Serialize(new { review.StatusCode, review.ModeratorComment });
            review.StatusCode = model.StatusCode;
            review.ModeratorComment = model.ModeratorComment;
            await _repository.UpdateReviewAsync(review);
            await _productRepository.RecalculateProductRatingAsync(review.ProductId!.Value);

            var after = JsonSerializer.Serialize(new { model.StatusCode, model.ModeratorComment });
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var accountName = $"{review.Account!.FirstName} {review.Account.LastName[0]}.";
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) статус відгуку покупця {accountName} на товар \"{review.ProductNameSnapshot}\" на {model.StatusCode}.", before, after);

            return "ok";
        }

        public async Task<string> AddReplyAsync(int reviewId, string text, int initiatorId)
        {
            var review = await _repository.GetReviewByIdAsync(reviewId);

            if (review == null)
                return "not_found";

            var existingReply = await _repository.GetReplyByReviewIdAsync(reviewId);

            if (existingReply != null)
                return "reply_exists";

            await _repository.AddReplyAsync(new ReviewReplyEntity
            {
                ReviewId = reviewId,
                Text = text,
                CreatedAt = DateTime.UtcNow
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var accountName = $"{review.Account!.FirstName} {review.Account.LastName[0]}.";
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) відповідь на відгук покупця {accountName} на товар \"{review.ProductNameSnapshot}\".");

            return "ok";
        }

        public async Task<string> UpdateReplyAsync(int replyId, string text, int initiatorId)
        {
            var reply = await _repository.GetReplyByIdAsync(replyId);

            if (reply == null)
                return "not_found";

            var before = reply.Text;

            reply.Text = text;
            reply.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateReplyAsync(reply);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = text;
            var accountName = $"{reply.Review!.Account!.FirstName} {reply.Review.Account.LastName[0]}.";
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) відповідь на відгук покупця {accountName} на товар \"{reply.Review.ProductNameSnapshot}\".", before, after);

            return "ok";
        }

        public async Task<string> DeleteReplyAsync(int replyId, int initiatorId)
        {
            var reply = await _repository.GetReplyByIdAsync(replyId);

            if (reply == null)
                return "not_found";

            await _repository.DeleteReplyAsync(reply);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var accountName = $"{reply.Review!.Account!.FirstName} {reply.Review.Account.LastName[0]}.";
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) відповідь на відгук покупця {accountName} на товар \"{reply.Review.ProductNameSnapshot}\".");

            return "ok";
        }

        public async Task<List<ResponseReviewStatusesModel>> GetReviewStatusesAsync()
        {
            var statuses = await _repository.GetReviewStatusesAsync();

            return statuses.Select(s => new ResponseReviewStatusesModel
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                IsActive = s.IsActive,
                ViewOrder = s.ViewOrder
            }).ToList();
        }

        public async Task<string> AddReviewStatusAsync(AddReviewStatusModel model, int initiatorId)
        {
            var codeExists = await _repository.ReviewStatusCodeExistsAsync(model.Code);

            if (codeExists)
                return "code_taken";

            await _repository.AddReviewStatusAsync(new ReviewStatusEntity
            {
                Code = model.Code,
                Name = model.Name,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) статус відгуку {model.Name}.");

            return "ok";
        }

        public async Task<string> UpdateReviewStatusAsync(int statusId, UpdateReviewStatusModel model, int initiatorId)
        {
            var status = await _repository.GetReviewStatusByIdAsync(statusId);

            if (status == null)
                return "not_found";

            var codeExists = await _repository.ReviewStatusCodeExistsAsync(model.Code);

            if (codeExists && status.Code != model.Code)
                return "code_taken";

            var before = JsonSerializer.Serialize(new { status.Code, status.Name, status.IsActive, status.ViewOrder });

            status.Code = model.Code;
            status.Name = model.Name;
            status.IsActive = model.IsActive;
            status.ViewOrder = model.ViewOrder;
            await _repository.UpdateReviewStatusAsync(status);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.Code, model.Name, model.IsActive, model.ViewOrder });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) статус відгуку {status.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteReviewStatusAsync(int statusId, int initiatorId)
        {
            var status = await _repository.GetReviewStatusByIdAsync(statusId);

            if (status == null)
                return "not_found";

            await _repository.DeleteReviewStatusAsync(status);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) статус відгуку {status.Name}.");

            return "ok";
        }

        public async Task ReorderReviewStatusesAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetReviewStatusesOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderReviewStatusesAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок статусів відгуків.", before, after);
        }

        public async Task<string> TakeReviewAsync(int reviewId, int employeeId)
        {
            var review = await _repository.GetReviewByIdAsync(reviewId);
            if (review == null)
                return "not_found";
            if (review.TakenByEmployeeId != null)
                return "already_taken";
            review.TakenByEmployeeId = employeeId;
            review.StatusCode = ReviewStatusCodes.Processing;
            await _repository.UpdateReviewAsync(review);
            await _notificationService.SendReviewTakenAsync();
            var initiator = await _repository.GetEmployeeByIdAsync(employeeId);
            var accountName = $"{review.Account!.FirstName} {review.Account.LastName[0]}";
            await _employeesActivityService.LogAsync(employeeId, $"{initiator!.Role!.Name} {initiator.Name}({employeeId}) взяв(ла) на модерацію відгук покупця {accountName} на товар \"{review.ProductNameSnapshot}\".");
            return "ok";
        }

        public async Task<string> ReleaseReviewAsync(int reviewId, int employeeId)
        {
            var review = await _repository.GetReviewByIdAsync(reviewId);
            if (review == null)
                return "not_found";
            if (review.TakenByEmployeeId != employeeId)
                return "forbidden";
            review.TakenByEmployeeId = null;
            await _repository.UpdateReviewAsync(review);
            await _notificationService.SendReviewReleasedAsync();
            var initiator = await _repository.GetEmployeeByIdAsync(employeeId);
            var accountName = $"{review.Account!.FirstName} {review.Account.LastName[0]}";
            await _employeesActivityService.LogAsync(employeeId, $"{initiator!.Role!.Name} {initiator.Name}({employeeId}) завершив(ла) модерацію відгуку покупця {accountName} на товар \"{review.ProductNameSnapshot}\".");
            return "ok";
        }

        public async Task<string> ResetReviewAsync(int reviewId, int initiatorId)
        {
            var review = await _repository.GetReviewByIdAsync(reviewId);
            if (review == null)
                return "not_found";
            if (review.TakenByEmployeeId == null)
                return "not_taken";
            review.TakenByEmployeeId = null;
            review.StatusCode = ReviewStatusCodes.Pending;
            await _repository.UpdateReviewAsync(review);
            await _notificationService.SendReviewResetAsync();
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var accountName = $"{review.Account!.FirstName} {review.Account.LastName[0]}";
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) скинув(ла) активну модерацію відгуку покупця {accountName} на товар \"{review.ProductNameSnapshot}\".");
            return "ok";
        }
    }
}