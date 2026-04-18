using SeedStore.Support.Store.Dictionary.NovaPoshtaDelivery.Interfaces;
using SeedStore.Support.Store.Dictionary.NovaPoshtaDelivery.Models;
using System.Text;
using System.Text.Json;

namespace SeedStore.Support.Store.Dictionary.NovaPoshtaDelivery.Services
{
    public class NovaPoshtaDeliveryService : INovaPoshtaDeliveryService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.novaposhta.ua/v2.0/json/";

        public NovaPoshtaDeliveryService(IConfiguration configuration, HttpClient httpClient)
        {
            _apiKey = configuration["NovaPoshta:ApiKey"]!;
            _httpClient = httpClient;
        }

        public async Task<List<NovaPoshtaSettlementModel>> SearchSettlementsAsync(string value)
        {
            var body = new
            {
                apiKey = _apiKey,
                modelName = "Address",
                calledMethod = "searchSettlements",
                methodProperties = new { CityName = value, Limit = 10 }
            };
            var response = await SendRequestAsync(body);
            if (response == null) return [];

            var data = response.RootElement.GetProperty("data");
            if (data.GetArrayLength() == 0) return [];

            var addresses = data[0].GetProperty("Addresses");
            return addresses.EnumerateArray()
                .Select(c => new NovaPoshtaSettlementModel
                {
                    Id = c.GetProperty("DeliveryCity").GetString() ?? string.Empty,
                    Name = c.GetProperty("MainDescription").GetString() ?? string.Empty,
                    Type = c.GetProperty("SettlementTypeCode").GetString() ?? string.Empty,
                    Region = c.GetProperty("Area").GetString() ?? string.Empty,
                    District = c.GetProperty("Region").GetString() ?? string.Empty
                })
                .ToList();
        }

        public async Task<List<NovaPoshtaWarehouseModel>> SearchWarehousesAsync(string settlementId, string? value)
        {
            var methodProperties = string.IsNullOrEmpty(value)
                ? (object)new { CityRef = settlementId, Limit = 50 }
                : new { CityRef = settlementId, FindByString = value, Limit = 20 };

            var body = new
            {
                apiKey = _apiKey,
                modelName = "Address",
                calledMethod = "getWarehouses",
                methodProperties
            };

            var response = await SendRequestAsync(body);
            if (response == null) return [];

            return response.RootElement
                .GetProperty("data")
                .EnumerateArray()
                .Select(w => new NovaPoshtaWarehouseModel
                {
                    Id = w.GetProperty("Ref").GetString() ?? string.Empty,
                    Name = w.GetProperty("Description").GetString() ?? string.Empty,
                    DigitalAddress = w.GetProperty("WarehouseIndex").GetString() ?? string.Empty
                })
                .ToList();
        }

        private async Task<JsonDocument?> SendRequestAsync(object body)
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(ApiUrl, content);
            if (!response.IsSuccessStatusCode) return null;
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(responseJson);
        }
    }
}