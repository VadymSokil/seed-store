namespace seed_store_api.Store.Support.TokenGeneration.Interfaces
{
    public interface ITokenGenerationService
    {
        string GenerateAccessToken(int accountId);
        string GenerateRefreshToken();
        string GenerateSecureToken();
        string GenerateVerificationCode();
        string GenerateOrderNumber();
        Guid GenerateLogId();
    }
}
