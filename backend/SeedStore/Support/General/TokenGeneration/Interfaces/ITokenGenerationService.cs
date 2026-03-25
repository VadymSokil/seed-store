namespace SeedStore.Support.General.TokenGeneration.Interfaces
{
    public interface ITokenGenerationService
    {
        string GenerateAccessToken(int accountId);
        string GenerateRefreshToken();
        string GenerateSecureToken();
        string GenerateVerificationCode();
        string GenerateOrderNumber();
        Guid GenerateLogId();
        string GenerateAccessToken(int accountId, string role, int priority);
    }
}
