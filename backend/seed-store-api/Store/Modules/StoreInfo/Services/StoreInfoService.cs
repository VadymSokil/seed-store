using Microsoft.Extensions.Caching.Memory;
using seed_store_api.Store.Modules.StoreInfo.Interfaces;
using seed_store_api.Store.Modules.StoreInfo.Models;

namespace seed_store_api.Store.Modules.StoreInfo.Services
{
    public class StoreInfoService : IStoreInfoService
    {
        private readonly IStoreInfoRepository _storeInfoRepository;
        private readonly IMemoryCache _cache;
        private const string DeliveryCacheKey = "delivery_variants";
        private const string PaymentCacheKey = "payment_variants";


        public StoreInfoService(IStoreInfoRepository storeInfoRepository, IMemoryCache cache)
        {
            _storeInfoRepository = storeInfoRepository;
            _cache = cache;
        }

        public async Task<List<VariantResponseModel>> GetDeliveryVariantsAsync()
        {
            if (_cache.TryGetValue(DeliveryCacheKey, out List<VariantResponseModel> cached))
                return cached;

            var deliveryVariants = await _storeInfoRepository.GetDeliveryVariantsAsync();
            var result = deliveryVariants.Select(dv => new VariantResponseModel
            {
                Id = dv.Id,
                Code = dv.Code,
                Name = dv.Name,
                Description = dv.Description,
                ViewOrder = dv.ViewOrder,
            }).ToList();

            _cache.Set(DeliveryCacheKey, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<List<VariantResponseModel>> GetPaymentVariantsAsync()
        {
            if (_cache.TryGetValue(PaymentCacheKey, out List<VariantResponseModel> cached))
                return cached;

            var paymentVariants = await _storeInfoRepository.GetPaymentVariantsAsync();
            var result = paymentVariants.Select(pv => new VariantResponseModel
            {
                Id = pv.Id,
                Code = pv.Code,
                Name = pv.Name,
                Description = pv.Description,
                ViewOrder = pv.ViewOrder,
            }).ToList();

            _cache.Set(PaymentCacheKey, result, TimeSpan.FromHours(1));
            return result;
        }
    }
}
