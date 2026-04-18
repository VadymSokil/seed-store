using Microsoft.Extensions.Caching.Memory;
using SeedStore.Store.StoreInfo.Interfaces;
using SeedStore.Store.StoreInfo.Models;
using SeedStore.Support.General.Constants.CacheKeys;

namespace SeedStore.Store.StoreInfo.Services
{
    public class StoreInfoService : IStoreInfoService
    {
        private readonly IStoreInfoRepository _storeInfoRepository;
        private readonly IMemoryCache _cache;

        public StoreInfoService(IStoreInfoRepository storeInfoRepository, IMemoryCache cache)
        {
            _storeInfoRepository = storeInfoRepository;
            _cache = cache;
        }

        public async Task<List<VariantResponseModel>> GetDeliveryVariantsAsync()
        {
            if (_cache.TryGetValue(CacheKeys.DeliveryVariants, out List<VariantResponseModel> cached))
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

            _cache.Set(CacheKeys.DeliveryVariants, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<List<VariantResponseModel>> GetPaymentVariantsAsync()
        {
            if (_cache.TryGetValue(CacheKeys.PaymentVariants, out List<VariantResponseModel> cached))
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

            _cache.Set(CacheKeys.PaymentVariants, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<string?> GetAboutPageAsync()
        {
            if (_cache.TryGetValue(CacheKeys.AboutPage, out string? cached))
                return cached;
            var entity = await _storeInfoRepository.GetAboutPageAsync();
            if (entity == null) return null;
            _cache.Set(CacheKeys.AboutPage, entity.Content, TimeSpan.FromHours(1));
            return entity.Content;
        }

        public async Task<string?> GetContactsAsync()
        {
            if (_cache.TryGetValue(CacheKeys.Contacts, out string? cached))
                return cached;
            var entity = await _storeInfoRepository.GetContactsAsync();
            if (entity == null) return null;
            _cache.Set(CacheKeys.Contacts, entity.Content, TimeSpan.FromHours(1));
            return entity.Content;
        }
    }
}
