using SeedStore.Admin.Authorization.Interfaces;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.General.PasswordHash.Interfaces;
using SeedStore.Support.General.TokenGeneration.Interfaces;

namespace SeedStore.Admin.Authorization.Services
{
    public class AdminAuthorizationService : IAdminAuthorizationService
    {
        private readonly IAdminAuthorizationRepository _repository;
        private readonly IPasswordHashService _passwordHashService;
        private readonly ITokenGenerationService _tokenGenerationService;
        private readonly IEmployeesActivityService _employeesActivityService;

        public AdminAuthorizationService(
            IAdminAuthorizationRepository repository,
            IPasswordHashService passwordHashService,
            ITokenGenerationService tokenGenerationService,
            IEmployeesActivityService employeesActivityService)
        {
            _repository = repository;
            _passwordHashService = passwordHashService;
            _tokenGenerationService = tokenGenerationService;
            _employeesActivityService = employeesActivityService;
        }

        public async Task<(string status, int? employeeId)> LoginAsync(string login, string password, HttpResponse response)
        {
            var employee = await _repository.GetEmployeeByLoginAsync(login);

            if (employee == null)
                return ("invalid_credentials", null);

            if (employee.IsBlocked)
                return ("blocked", null);

            if (!_passwordHashService.Verify(password, employee.PasswordHash))
                return ("invalid_credentials", null);

            var accessToken = _tokenGenerationService.GenerateAccessToken(employee.Id, employee.Role!.Code, employee.Role!.Priority);
            var refreshToken = _tokenGenerationService.GenerateRefreshToken();

            await _repository.AddRefreshTokenAsync(new EmployeeRefreshTokenEntity
            {
                EmployeeId = employee.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            });

            response.Cookies.Append("admin_access_token", accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            response.Cookies.Append("admin_refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30)
            });

            await _employeesActivityService.LogAsync(employee.Id, $"{employee.Role!.Name} {employee.Name}({employee.Id}) авторизувався(лась) в системі.");

            return ("ok", employee.Id);
        }

        public async Task<string> LogoutAsync(HttpRequest request, HttpResponse response)
        {
            var refreshToken = request.Cookies["admin_refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return "not_authorized";

            var token = await _repository.GetRefreshTokenAsync(refreshToken);

            if (token != null)
            {
                var employee = await _repository.GetEmployeeByIdAsync(token.EmployeeId);
                await _repository.DeleteRefreshTokenAsync(token);
                if (employee != null)
                    await _employeesActivityService.LogAsync(employee.Id, $"{employee.Role!.Name} {employee.Name}({employee.Id}) вийшов(ла) з системи.");
            }

            response.Cookies.Delete("admin_access_token");
            response.Cookies.Delete("admin_refresh_token");

            return "ok";
        }

        public async Task<string> RefreshTokenAsync(HttpRequest request, HttpResponse response)
        {
            var refreshToken = request.Cookies["admin_refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                return "not_authorized";

            var token = await _repository.GetRefreshTokenAsync(refreshToken);

            if (token == null)
                return "not_authorized";

            if (token.ExpiresAt < DateTime.UtcNow)
            {
                await _repository.DeleteRefreshTokenAsync(token);
                return "token_expired";
            }

            var employee = await _repository.GetEmployeeByIdAsync(token.EmployeeId);

            if (employee == null || employee.IsBlocked)
                return "not_authorized";

            var newAccessToken = _tokenGenerationService.GenerateAccessToken(employee.Id, employee.Role!.Code, employee.Role!.Priority);
            var newRefreshToken = _tokenGenerationService.GenerateRefreshToken();

            await _repository.DeleteRefreshTokenAsync(token);
            await _repository.AddRefreshTokenAsync(new EmployeeRefreshTokenEntity
            {
                EmployeeId = employee.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            });

            response.Cookies.Append("admin_access_token", newAccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(60)
            });

            response.Cookies.Append("admin_refresh_token", newRefreshToken, new CookieOptions
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