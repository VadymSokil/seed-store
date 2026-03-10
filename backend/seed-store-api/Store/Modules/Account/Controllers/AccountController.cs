using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using seed_store_api.Store.Modules.Account.Interfaces;
using seed_store_api.Store.Modules.Account.Models;
using System.Security.Claims;

namespace seed_store_api.Store.Modules.Account.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Authorize]
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountInfo()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.GetAccountInfoAsync(accountId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("name")]
        public async Task<IActionResult> ChangeName([FromBody] ChangeNameModel model)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.ChangeNameAsync(accountId, model);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("email")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailModel model)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.ChangeEmailAsync(accountId, model.NewEmail);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "email_taken" => Conflict("email_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmailChange([FromBody] ConfirmEmailChangeModel model)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.ConfirmEmailChangeAsync(accountId, model);

            return result switch
            {
                "ok" => Ok(),
                "invalid_code" => BadRequest("invalid_code"),
                "code_expired" => BadRequest("code_expired"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("phone")]
        public async Task<IActionResult> ChangePhone([FromBody] ChangePhoneModel model)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.ChangePhoneAsync(accountId, model?.PhoneNumber);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.ChangePasswordAsync(accountId, model);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "invalid_password" => BadRequest("invalid_password"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _accountService.DeleteAccountAsync(accountId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }
    }
}
