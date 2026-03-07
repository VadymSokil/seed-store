using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using seed_store_api.Store.Modules.Reviews.Interfaces;
using seed_store_api.Store.Modules.Reviews.Models;
using System.Security.Claims;

namespace seed_store_api.Store.Modules.Reviews.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Route("reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsService _reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            _reviewsService = reviewsService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            var result = await _reviewsService.GetProductReviewsAsync(productId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("account")]
        public async Task<IActionResult> GetAccountReviews()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _reviewsService.GetAccountReviewsAsync(accountId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewModel model)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _reviewsService.AddReviewAsync(accountId, model);

            return result switch
            {
                "ok" => Ok(),
                "product_not_found" => NotFound("product_not_found"),
                "review_exists" => Conflict("review_exists"),
                _ => StatusCode(500)
            };
        }

        [Authorize]
        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ChangeReviewModel model)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _reviewsService.UpdateReviewAsync(accountId, reviewId, model);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound("not_found"),
                "forbidden" => StatusCode(403, "forbidden"),
                _ => StatusCode(500)
            };
        }

        [Authorize]
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _reviewsService.DeleteReviewAsync(accountId, reviewId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound("not_found"),
                "forbidden" => StatusCode(403, "forbidden"),
                _ => StatusCode(500)
            };
        }
    }
}
