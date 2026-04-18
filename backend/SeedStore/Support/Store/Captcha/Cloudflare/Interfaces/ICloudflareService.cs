namespace SeedStore.Support.Store.Captcha.Cloudflare.Interfaces
{
    public interface ICloudflareService
    {
        Task<bool> VerifyTurnstileAsync(string token);
    }
}
