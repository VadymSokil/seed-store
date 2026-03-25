using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.Account.Interfaces;
using SeedStore.Admin.Account.Models;
using System.Security.Claims;

namespace SeedStore.Admin.Account.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/admin/account")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        private readonly IAdminAccountService _accountService;

        public AdminAccountController(IAdminAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize(Policy = "Permission:accounts.manage")]
        public async Task<IActionResult> GetEmployees()
        {
            var result = await _accountService.GetEmployeesAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Permission:accounts.manage")]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.AddEmployeeAsync(model, initiatorId);
            return result switch
            {
                "ok" => Ok(),
                "login_taken" => Conflict("login_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/name")]
        [Authorize(Policy = "Permission:accounts.manage")]
        public async Task<IActionResult> ChangeEmployeeName(int id, [FromBody] ChangeEmployeeNameModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.ChangeEmployeeNameAsync(id, model.Name, initiatorId);
            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "forbidden" => StatusCode(403, "forbidden"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/password")]
        [Authorize(Policy = "Permission:accounts.manage")]
        public async Task<IActionResult> ChangeEmployeePassword(int id, [FromBody] ChangeEmployeePasswordModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.ChangeEmployeePasswordAsync(id, model.Password, initiatorId);
            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "forbidden" => StatusCode(403, "forbidden"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:accounts.manage")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.DeleteEmployeeAsync(id, initiatorId);
            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "forbidden" => StatusCode(403, "forbidden"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/block")]
        [Authorize(Policy = "Permission:accounts.manage")]
        public async Task<IActionResult> BlockEmployee(int id, [FromBody] BlockEmployeeModel model)
        {
            var blockedByEmployeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.BlockEmployeeAsync(id, blockedByEmployeeId, model.Reason);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "already_blocked" => Conflict("already_blocked"),
                "forbidden" => StatusCode(403, "forbidden"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/unblock")]
        [Authorize(Policy = "Permission:accounts.manage")]
        public async Task<IActionResult> UnblockEmployee(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.UnblockEmployeeAsync(id, initiatorId);
            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "not_blocked" => Conflict("not_blocked"),
                "forbidden" => StatusCode(403, "forbidden"),
                _ => StatusCode(500)
            };
        }
    }
}