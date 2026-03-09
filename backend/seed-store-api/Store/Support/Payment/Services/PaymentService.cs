using Microsoft.Extensions.Configuration;
using seed_store_api.Store.Support.Payment.Interfaces;
using seed_store_api.Store.Support.Payment.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace seed_store_api.Store.Support.Payment.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly string _publicKey;
        private readonly string _privateKey;

        public PaymentService(IConfiguration configuration)
        {
            _publicKey = configuration["LiqPay:PublicKey"]!;
            _privateKey = configuration["LiqPay:PrivateKey"]!;
        }

        public (string data, string signature) GetPaymentData(string orderNumber, decimal amount, string description)
        {
            var parameters = new Dictionary<string, object>
            {
                { "version", 3 },
                { "public_key", _publicKey },
                { "action", "pay" },
                { "amount", amount },
                { "currency", "UAH" },
                { "description", description },
                { "order_id", orderNumber }
            };

            var json = JsonSerializer.Serialize(parameters);
            var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            var signature = GenerateSignature(data);

            return (data, signature);
        }

        public bool ValidateCallback(string data, string signature)
        {
            var expectedSignature = GenerateSignature(data);
            return expectedSignature == signature;
        }

        private string GenerateSignature(string data)
        {
            var raw = _privateKey + data + _privateKey;
            var hash = SHA1.HashData(Encoding.UTF8.GetBytes(raw));
            return Convert.ToBase64String(hash);
        }

        public LiqPayCallbackDataModel DecodeCallbackData(string data)
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(data));
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new LiqPayCallbackDataModel
            {
                OrderId = root.GetProperty("order_id").GetString() ?? string.Empty,
                PaymentId = root.GetProperty("payment_id").GetInt64(),
                LiqPayOrderId = root.GetProperty("liqpay_order_id").GetString() ?? string.Empty,
                Status = root.GetProperty("status").GetString() ?? string.Empty,
                Amount = root.GetProperty("amount").GetDecimal(),
                Currency = root.GetProperty("currency").GetString() ?? string.Empty,
                ErrCode = root.TryGetProperty("err_code", out var errCode) ? errCode.GetString() : null,
                ErrDescription = root.TryGetProperty("err_description", out var errDesc) ? errDesc.GetString() : null
            };
        }
    }
}