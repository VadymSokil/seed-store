using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.Authorization.Interfaces;
using SeedStore.Admin.Authorization.Models;

namespace SeedStore.Admin.Authorization.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Route("api/admin/auth")]
    [ApiController]
    public class AdminAuthorizationController : ControllerBase
    {
        private readonly IAdminAuthorizationService _authorizationService;

        public AdminAuthorizationController(IAdminAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var (status, employeeId) = await _authorizationService.LoginAsync(model.Login, model.Password, Response);

            return status switch
            {
                "ok" => Ok(new { employeeId }),
                "invalid_credentials" => Unauthorized("invalid_credentials"),
                "blocked" => Forbid(),
                _ => StatusCode(500)
            };
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authorizationService.LogoutAsync(Request, Response);

            return result switch
            {
                "ok" => Ok(),
                "not_authorized" => Unauthorized("not_authorized"),
                _ => StatusCode(500)
            };
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authorizationService.RefreshTokenAsync(Request, Response);

            return result switch
            {
                "ok" => Ok(),
                "not_authorized" => Unauthorized("not_authorized"),
                "token_expired" => Unauthorized("token_expired"),
                _ => StatusCode(500)
            };
        }
    }
}