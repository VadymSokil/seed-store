using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Context;
using seed_store_api.Database.Entities.Store.Modules.StoreInfo;
using seed_store_api.Store.Modules.StoreInfo.Interfaces;

namespace seed_store_api.Store.Modules.StoreInfo.Repositories
{
    public class StoreInfoRepository : IStoreInfoRepository
    {
        private readonly AppDbContext _context;

        public StoreInfoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DeliveryVariantEntity>> GetDeliveryVariantsAsync()
        {
            return await _context.DeliveryVariants.Where(c => c.IsActive).OrderBy(c => c.ViewOrder).ToListAsync();
        }

        public async Task<List<PaymentVariantEntity>> GetPaymentVariantsAsync()
        {
            return await _context.PaymentVariants.Where(c => c.IsActive).OrderBy(c => c.ViewOrder).ToListAsync();
        }
    }
}
