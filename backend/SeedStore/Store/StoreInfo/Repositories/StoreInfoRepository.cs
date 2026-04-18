using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Store.StoreInfo;
using SeedStore.Store.StoreInfo.Interfaces;

namespace SeedStore.Store.StoreInfo.Repositories
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

        public async Task<AboutPageEntity?> GetAboutPageAsync()
        {
            return await _context.AboutPages.FirstOrDefaultAsync();
        }

        public async Task<ContactsEntity?> GetContactsAsync()
        {
            return await _context.Contacts.FirstOrDefaultAsync();
        }
    }
}
