using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.Reviews.Interfaces;
using SeedStore.Admin.Reviews.Models;
using SeedStore.Support.Admin.Reorder.Models;
using System.Security.Claims;

namespace SeedStore.Admin.Reviews.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/admin/reviews")]
    [ApiController]
    public class AdminReviewsController : ControllerBase
    {
        private readonly IAdminReviewsService _adminReviewsService;

        public AdminReviewsController(IAdminReviewsService adminReviewsService)
        {
            _adminReviewsService = adminReviewsService;
        }

        [HttpGet]
        [Authorize(Policy = "Permission:reviews.moderate")]
        public async Task<IActionResult> GetReviews([FromQuery] string? statusCode)
        {
            var result = await _adminReviewsService.GetReviewsAsync(statusCode);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Permission:reviews.moderate")]
        public async Task<IActionResult> GetReview(int id)
        {
            var result = await _adminReviewsService.GetReviewAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}/moderate")]
        [Authorize(Policy = "Permission:reviews.moderate")]
        public async Task<IActionResult> ModerateReview(int id, [FromBody] ModerateReviewModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.ModerateReviewAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPost("{id}/reply")]
        [Authorize(Policy = "Permission:reviews.reply")]
        public async Task<IActionResult> AddReply(int id, [FromBody] ReplyTextModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.AddReplyAsync(id, model.Text, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "reply_exists" => Conflict("reply_exists"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("replies/{replyId}")]
        [Authorize(Policy = "Permission:reviews.reply")]
        public async Task<IActionResult> UpdateReply(int replyId, [FromBody] ReplyTextModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.UpdateReplyAsync(replyId, model.Text, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("replies/{replyId}")]
        [Authorize(Policy = "Permission:reviews.reply")]
        public async Task<IActionResult> DeleteReply(int replyId)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.DeleteReplyAsync(replyId, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("statuses")]
        [Authorize(Policy = "Permission:reviews.moderate")]
        public async Task<IActionResult> GetReviewStatuses()
        {
            var result = await _adminReviewsService.GetReviewStatusesAsync();
            return Ok(result);
        }

        [HttpPost("statuses")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> AddReviewsStatus([FromBody] AddReviewStatusModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.AddReviewStatusAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("statuses/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> UpdateReviewStatus(int id, [FromBody] UpdateReviewStatusModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.UpdateReviewStatusAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("statuses/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> DeleteReviewsStatus(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.DeleteReviewStatusAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("reorder")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> ReorderReviewStatuses([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminReviewsService.ReorderReviewStatusesAsync(items, initiatorId);
            return Ok();
        }

        [HttpPost("{id}/take")]
        [Authorize(Policy = "Permission:reviews.moderate")]
        public async Task<IActionResult> TakeReview(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.TakeReviewAsync(id, employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "already_taken" => Conflict("already_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPost("{id}/release")]
        [Authorize(Policy = "Permission:reviews.moderate")]
        public async Task<IActionResult> ReleaseReview(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.ReleaseReviewAsync(id, employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "forbidden" => Forbid(),
                _ => StatusCode(500)
            };
        }

        [HttpPost("{id}/reset")]
        [Authorize(Policy = "Permission:reviews.moderate")]
        public async Task<IActionResult> ResetReview(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminReviewsService.ResetReviewAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "not_taken" => BadRequest("not_taken"),
                _ => StatusCode(500)
            };
        }
    }
}