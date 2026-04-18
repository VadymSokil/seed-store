using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.StoreInfo.Interfaces;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.StoreInfo;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.StoreInfo.Repositories
{
    public class AdminStoreInfoRepository : IAdminStoreInfoRepository
    {
        private readonly AppDbContext _context;

        public AdminStoreInfoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DeliveryVariantEntity>> GetDeliveryVariantsAsync()
        {
            return await _context.DeliveryVariants.OrderBy(d => d.ViewOrder).ToListAsync();
        }

        public async Task<DeliveryVariantEntity?> GetDeliveryVariantByIdAsync(int id)
        {
            return await _context.DeliveryVariants.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> DeliveryVariantCodeExistsAsync(string code)
        {
            return await _context.DeliveryVariants.AnyAsync(d => d.Code == code);
        }

        public async Task AddDeliveryVariantAsync(DeliveryVariantEntity entity)
        {
            entity.ViewOrder = await GetNextDeliveryVariantViewOrderAsync();
            await _context.DeliveryVariants.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeliveryVariantAsync(DeliveryVariantEntity entity)
        {
            _context.DeliveryVariants.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeliveryVariantAsync(DeliveryVariantEntity entity)
        {
            _context.DeliveryVariants.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PaymentVariantEntity>> GetPaymentVariantsAsync()
        {
            return await _context.PaymentVariants.OrderBy(p => p.ViewOrder).ToListAsync();
        }

        public async Task<PaymentVariantEntity?> GetPaymentVariantByIdAsync(int id)
        {
            return await _context.PaymentVariants.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> PaymentVariantCodeExistsAsync(string code)
        {
            return await _context.PaymentVariants.AnyAsync(p => p.Code == code);
        }

        public async Task AddPaymentVariantAsync(PaymentVariantEntity entity)
        {
            entity.ViewOrder = await GetNextPaymentVariantViewOrderAsync();
            await _context.PaymentVariants.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentVariantAsync(PaymentVariantEntity entity)
        {
            _context.PaymentVariants.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaymentVariantAsync(PaymentVariantEntity entity)
        {
            _context.PaymentVariants.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        private async Task<int> GetNextDeliveryVariantViewOrderAsync()
        {
            return (await _context.DeliveryVariants.MaxAsync(p => p.ViewOrder) ?? 0) + 1;
        }

        private async Task<int> GetNextPaymentVariantViewOrderAsync()
        {
            return (await _context.PaymentVariants.MaxAsync(p => p.ViewOrder) ?? 0) + 1;
        }

        public async Task ReorderDeliveryVariantsAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.DeliveryVariants
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderPaymentVariantsAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.PaymentVariants
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task<List<ReorderItemModel>> GetDeliveryVariantsOrderAsync()
        {
            return await _context.DeliveryVariants
                .OrderBy(d => d.ViewOrder)
                .Select(d => new ReorderItemModel { Id = d.Id, ViewOrder = d.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<ReorderItemModel>> GetPaymentVariantsOrderAsync()
        {
            return await _context.PaymentVariants
                .OrderBy(p => p.ViewOrder)
                .Select(p => new ReorderItemModel { Id = p.Id, ViewOrder = p.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<AboutPageEntity?> GetAboutPageAsync()
        {
            return await _context.AboutPages.FirstOrDefaultAsync();
        }

        public async Task UpdateAboutPageAsync(AboutPageEntity entity)
        {
            _context.AboutPages.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ContactsEntity?> GetContactsAsync()
        {
            return await _context.Contacts.FirstOrDefaultAsync();
        }

        public async Task UpdateContactsAsync(ContactsEntity entity)
        {
            _context.Contacts.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}