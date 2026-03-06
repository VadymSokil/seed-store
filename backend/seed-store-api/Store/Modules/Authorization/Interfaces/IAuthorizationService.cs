using seed_store_api.Store.Modules.Authorization.Models;

namespace seed_store_api.Store.Modules.Authorization.Interfaces
{
    public interface IAuthorizationService
    {
        Task<string> RegistrationAsync(RegistrationModel registration);
        Task<string> VerifyEmailAsync(VerifyEmailModel model);
        Task<string> ResendVerificationCodeAsync(string email);
        Task<string> LoginAsync(LoginModel model, HttpResponse response);
        Task<string> ForgotPasswordAsync(string email);
        Task<string> VerifyResetCodeAsync(VerifyResetCodeModel model);
        Task<string> ResetPasswordAsync(ResetPasswordModel model);
        Task<string> ResendResetCodeAsync(string email);
        Task<string> LogoutAsync(HttpRequest request, HttpResponse response);
        Task<string> RefreshTokenAsync(HttpRequest request, HttpResponse response);
    }
}
