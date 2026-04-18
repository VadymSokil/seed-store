using Microsoft.Extensions.Configuration;
using SeedStore.Support.Store.Captcha.Cloudflare.Interfaces;
using System.Text.Json;

namespace SeedStore.Support.Store.Captcha.Cloudflare.Services
{
    public class CloudflareService : ICloudflareService
    {
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;

        public CloudflareService(IConfiguration configuration, HttpClient httpClient)
        {
            _secretKey = configuration["Turnstile:SecretKey"]!;
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyTurnstileAsync(string token)
        {
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", _secretKey),
                new KeyValuePair<string, string>("response", token)
            });

            var response = await _httpClient.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", formData);
            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("success").GetBoolean();
        }
    }
}