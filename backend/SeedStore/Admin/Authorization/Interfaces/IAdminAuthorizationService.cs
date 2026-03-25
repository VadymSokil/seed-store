namespace SeedStore.Admin.Authorization.Interfaces
{
    public interface IAdminAuthorizationService
    {
        Task<(string status, int? employeeId)> LoginAsync(string login, string password, HttpResponse response);
        Task<string> LogoutAsync(HttpRequest request, HttpResponse response);
        Task<string> RefreshTokenAsync(HttpRequest request, HttpResponse response);
    }
}
