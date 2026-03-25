using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.Stats.Interfaces;
using SeedStore.Admin.Stats.Models;
using System.Security.Claims;

namespace SeedStore.Admin.Stats.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/admin/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;

        public StatsController(IStatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpPost("shift/start")]
        [Authorize(Policy = "Permission:stats.view")]
        public async Task<IActionResult> StartShift()
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _statsService.StartShiftAsync(employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "already_on_shift" => Conflict("already_on_shift"),
                _ => StatusCode(500)
            };
        }

        [HttpPost("shift/end")]
        [Authorize(Policy = "Permission:stats.view")]
        public async Task<IActionResult> EndShift()
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _statsService.EndShiftAsync(employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "not_on_shift" => Conflict("not_on_shift"),
                _ => StatusCode(500)
            };
        }

        [HttpGet("active-employees")]
        [Authorize(Policy = "Permission:stats.view")]
        public async Task<IActionResult> GetActiveEmployees()
        {
            var result = await _statsService.GetActiveEmployeesAsync();
            return Ok(result);
        }

        [HttpGet("activity")]
        [Authorize(Policy = "Permission:stats.view")]
        public async Task<IActionResult> GetActivity([FromQuery] ActivityFilterModel filter)
        {
            var result = await _statsService.GetActivityAsync(filter);
            return Ok(result);
        }

        [HttpGet("orders")]
        [Authorize(Policy = "Permission:stats.view")]
        public async Task<IActionResult> GetOrdersStats([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var result = await _statsService.GetOrdersStatsAsync(dateFrom, dateTo);
            return Ok(result);
        }

        [HttpGet("revenue")]
        [Authorize(Policy = "Permission:stats.view")]
        public async Task<IActionResult> GetRevenue([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            var result = await _statsService.GetRevenueStatsAsync(dateFrom, dateTo);
            return Ok(result);
        }

        [HttpGet("active-processing")]
        [Authorize(Policy = "Permission:stats.view")]
        public async Task<IActionResult> GetActiveProcessing()
        {
            var result = await _statsService.GetActiveProcessingAsync();
            return Ok(result);
        }
    }
}