using SeedStore.Support.Store.Dictionary.NovaPoshtaDelivery.Models;

namespace SeedStore.Support.Store.Dictionary.NovaPoshtaDelivery.Interfaces
{
    public interface INovaPoshtaDeliveryService
    {
        Task<List<NovaPoshtaSettlementModel>> SearchSettlementsAsync(string value);
        Task<List<NovaPoshtaWarehouseModel>> SearchWarehousesAsync(string settlementId, string? value);
    }
}