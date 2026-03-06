using seed_store_api.Database.Entities.Store.Modules.Account;
using seed_store_api.Store.Modules.Authorization.Interfaces;
using seed_store_api.Store.Modules.Authorization.Models;
using seed_store_api.Store.Support.Email.Interfaces;
using seed_store_api.Store.Support.TokenGeneration.Interfaces;
using seed_store_api.Store.Support.PasswordHash.Interfaces;

namespace seed_store_api.Store.Modules.Authorization.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ITokenGenerationService _tokenGenerationService;

        public AuthorizationService(IAuthorizationRepository authorizationRepository, IEmailService emailService, IPasswordHashService passwordHashService, ITokenGenerationService tokenGenerationService)
        {
            _authorizationRepository = authorizationRepository;
            _emailService = emailService;
            _passwordHashService = passwordHashService;
            _tokenGenerationService = tokenGenerationService;
        }

        public async Task<string> RegistrationAsync(RegistrationModel model)
        {
            if (model.Password != model.ConfirmPassword)
                return "passwords_mismatch";

            var accountExists = await _authorizationRepository.AccountExistsAsync(model.Email);
            if (accountExists)
                return "email_taken";

            var pendingExists = await _authorizationRepository.PendingAccountExistsAsync(model.Email);
            if (pendingExists)
                return "email_pending";

            var passwordHash = _passwordHashService.Hash(model.Password);
            var code = _tokenGenerationService.GenerateVerificationCode();

            var pendingAccount = new PendingAccountEntity
            {
                Email = model.Email,
                PasswordHash = passwordHash,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _authorizationRepository.AddPendingAccountAsync(pendingAccount);

            await _emailService.SendEmailAsync(
                model.Email,
                "Підтвердження реєстрації",
                $"<p>Ваш код підтвердження: <b>{code}</b></p><p>Код дійсний 15 хвилин.</p>"
            );

            return "ok";
        }

        public async Task<string> VerifyEmailAsync(VerifyEmailModel model)
        {
            var pendingAccount = await _authorizationRepository.GetPendingAccountAsync(model.Email, model.Code);

            if (pendingAccount == null)
                return "invalid_code";

            if (pendingAccount.ExpiresAt < DateTime.UtcNow)
                return "code_expired";

            var account = new AccountEntity
            {
                Email = pendingAccount.Email,
                PasswordHash = pendingAccount.PasswordHash,
                FirstName = pendingAccount.FirstName,
                LastName = pendingAccount.LastName,
                MiddleName = pendingAccount.MiddleName,
                CreatedAt = DateTime.UtcNow
            };

            var accountId = await _authorizationRepository.AddAccountAsync(account);
            await _authorizationRepository.DeletePendingAccountAsync(pendingAccount);

            return accountId.ToString();
        }

        public async Task<string> ResendVerificationCodeAsync(string email)
        {
            var pendingAccount = await _authorizationRepository.GetPendingAccountByEmailAsync(email);

            if (pendingAccount == null)
                return "not_found";

            var code = _tokenGenerationService.GenerateVerificationCode();
            pendingAccount.Code = code;
            pendingAccount.ExpiresAt = DateTime.UtcNow.AddMinutes(15);

            await _authorizationRepository.UpdatePendingAccountCodeAsync(pendingAccount);

            await _emailService.SendEmailAsync(
                email,
                "Підтвердження реєстрації",
                $"<p>Ваш новий код підтвердження: <b>{code}</b></p><p>Код дійсний 15 хвилин.</p>"
            );

            return "ok";
        }

        public async Task<string> LoginAsync(LoginModel model, HttpResponse response)
        {
            var account = await _authorizationRepository.GetAccountByEmailAsync(model.Email);

            if (account == null)
                return "invalid_credentials";

            if (!_passwordHashService.Verify(model.Password, account.PasswordHash))
                return "invalid_credentials";

            var accessToken = _tokenGenerationService.GenerateAccessToken(account.Id);
            var refreshToken = _tokenGenerationService.GenerateRefreshToken();

            await _authorizationRepository.AddRefreshTokenAsync(new RefreshTokenEntity
            {
                AccountId = account.Id,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            });

            var refreshTokenExpires = model.RememberMe ? DateTime.UtcNow.AddDays(30) : (DateTime?)null;

            response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshTokenExpires
            });

            return account.Id.ToString();
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var account = await _authorizationRepository.GetAccountByEmailAsync(email);

            if (account == null)
                return "not_found";

            var code = _tokenGenerationService.GenerateVerificationCode();

            var entity = new PasswordResetRequestEntity
            {
                AccountId = account.Id,
                Code = code,
                Token = string.Empty,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _authorizationRepository.UpsertPasswordResetRequestAsync(entity);

            await _emailService.SendEmailAsync(
                email,
                "Скидання пароля",
                $"<p>Ваш код для скидання пароля: <b>{code}</b></p><p>Код дійсний 15 хвилин.</p>"
            );

            return "ok";
        }

        public async Task<string> VerifyResetCodeAsync(VerifyResetCodeModel model)
        {
            var request = await _authorizationRepository.GetPasswordResetRequestAsync(model.Email, model.Code);

            if (request == null)
                return "invalid_code";

            if (request.ExpiresAt < DateTime.UtcNow)
                return "code_expired";

            var token = _tokenGenerationService.GenerateSecureToken();
            request.Token = token;

            await _authorizationRepository.UpdatePasswordResetTokenAsync(request);

            return token;
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordModel model)
        {
            var request = await _authorizationRepository.GetPasswordResetRequestByTokenAsync(model.Email, model.Token);

            if (request == null)
                return "invalid_token";

            var passwordHash = _passwordHashService.Hash(model.Password);

            await _authorizationRepository.UpdateAccountPasswordAsync(request.AccountId, passwordHash);
            await _authorizationRepository.DeleteRefreshTokenByAccountIdAsync(request.AccountId);
            await _authorizationRepository.DeletePasswordResetRequestAsync(request);

            return "ok";
        }

        public async Task<string> ResendResetCodeAsync(string email)
        {
            var request = await _authorizationRepository.GetPasswordResetRequestByEmailAsync(email);

            if (request == null)
                return "not_found";

            var code = _tokenGenerationService.GenerateVerificationCode();
            request.Code = code;
            request.Token = string.Empty;
            request.ExpiresAt = DateTime.UtcNow.AddMinutes(15);

            await _authorizationRepository.UpdatePasswordResetTokenAsync(request);

            await _emailService.SendEmailAsync(
                email,
                "Скидання пароля",
                $"<p>Ваш новий код для скидання пароля: <b>{code}</b></p><p>Код дійсний 15 хвилин.</p>"
            );

            return "ok";
        }

        public async Task<string> LogoutAsync(HttpRequest request, HttpResponse response)
        {
            var refreshToken = request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return "not_authorized";

            var token = await _authorizationRepository.GetRefreshTokenAsync(refreshToken);

            if (token != null)
                await _authorizationRepository.DeleteRefreshTokenAsync(token);

            response.Cookies.Delete("access_token");
            response.Cookies.Delete("refresh_token");

            return "ok";
        }

        public async Task<string> RefreshTokenAsync(HttpRequest request, HttpResponse response)
        {
            var refreshToken = request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return "not_authorized";

            var token = await _authorizationRepository.GetRefreshTokenAsync(refreshToken);

            if (token == null)
                return "not_authorized";

            if (token.ExpiresAt < DateTime.UtcNow)
            {
                await _authorizationRepository.DeleteRefreshTokenAsync(token);
                return "token_expired";
            }

            var newAccessToken = _tokenGenerationService.GenerateAccessToken(token.AccountId);
            var newRefreshToken = _tokenGenerationService.GenerateRefreshToken();

            await _authorizationRepository.DeleteRefreshTokenByAccountIdAsync(token.AccountId);

            await _authorizationRepository.AddRefreshTokenAsync(new RefreshTokenEntity
            {
                AccountId = token.AccountId,
                Token = newRefreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            });

            response.Cookies.Append("access_token", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            response.Cookies.Append("refresh_token", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30)
            });

            return "ok";
        }
    }
}