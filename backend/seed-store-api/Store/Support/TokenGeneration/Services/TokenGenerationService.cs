using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using seed_store_api.Store.Support.TokenGeneration.Interfaces;

namespace seed_store_api.Store.Support.TokenGeneration.Services
{
    public class TokenGenerationService : ITokenGenerationService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;

        public TokenGenerationService(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:SecretKey"]!;
            _issuer = configuration["Jwt:Issuer"]!;
            _audience = configuration["Jwt:Audience"]!;
            _accessTokenExpirationMinutes = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"]!);
            _refreshTokenExpirationDays = int.Parse(configuration["Jwt:RefreshTokenExpirationDays"]!);
        }

        public string GenerateAccessToken(int accountId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, accountId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string GenerateSecureToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public string GenerateOrderNumber()
        {
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = RandomNumberGenerator.GetInt32(1000, 9999);
            return $"ORD-{datePart}-{randomPart}";
        }
        public Guid GenerateLogId()
        {
            return Guid.NewGuid();
        }
    }
}