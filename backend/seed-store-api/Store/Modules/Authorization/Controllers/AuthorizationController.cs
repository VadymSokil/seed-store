using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using seed_store_api.Store.Modules.Authorization.Interfaces;
using seed_store_api.Store.Modules.Authorization.Models;


namespace seed_store_api.Store.Modules.Authorization.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Route("authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationModel registration)
        {
            var result = await _authorizationService.RegistrationAsync(registration);
            return result switch
            {
                "ok" => Ok(),
                "email_taken" => Conflict("email_taken"),
                "email_pending" => Conflict("email_pending"),
                "passwords_mismatch" => BadRequest("passwords_mismatch"),
                _ => StatusCode(500)
            };
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailModel model)
        {
            var result = await _authorizationService.VerifyEmailAsync(model);

            return result switch
            {
                "invalid_code" => BadRequest("invalid_code"),
                "code_expired" => BadRequest("code_expired"),
                _ => Ok(int.Parse(result))
            };
        }

        [HttpPost("resend-verification-code")]
        public async Task<IActionResult> ResendVerificationCode([FromBody] string email)
        {
            var result = await _authorizationService.ResendVerificationCodeAsync(email);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound("not_found"),
                _ => StatusCode(500)
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authorizationService.LoginAsync(model, Response);

            return result switch
            {
                "invalid_credentials" => Unauthorized("invalid_credentials"),
                _ => Ok(int.Parse(result))
            };
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _authorizationService.ForgotPasswordAsync(email);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound("not_found"),
                _ => StatusCode(500)
            };
        }

        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeModel model)
        {
            var result = await _authorizationService.VerifyResetCodeAsync(model);

            return result switch
            {
                "invalid_code" => BadRequest("invalid_code"),
                "code_expired" => BadRequest("code_expired"),
                _ => Ok(result)
            };
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = await _authorizationService.ResetPasswordAsync(model);

            return result switch
            {
                "ok" => Ok(),
                "invalid_token" => BadRequest("invalid_token"),
                _ => StatusCode(500)
            };
        }

        [HttpPost("resend-reset-code")]
        public async Task<IActionResult> ResendResetCode([FromBody] string email)
        {
            var result = await _authorizationService.ResendResetCodeAsync(email);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound("not_found"),
                _ => StatusCode(500)
            };
        }

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
