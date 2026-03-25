using Microsoft.Extensions.Caching.Memory;
using SeedStore.Admin.StoreInfo.Interfaces;
using SeedStore.Admin.StoreInfo.Models;
using SeedStore.Database.Entities.Store.StoreInfo;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.Admin.Reorder.Models;
using SeedStore.Support.General.Constants.CacheKeys;
using System.Text.Json;

namespace SeedStore.Admin.StoreInfo.Services
{
    public class AdminStoreInfoService : IAdminStoreInfoService
    {
        private readonly IAdminStoreInfoRepository _repository;
        private readonly IEmployeesActivityService _employeesActivityService;
        private readonly IMemoryCache _cache;

        public AdminStoreInfoService(IAdminStoreInfoRepository repository, IEmployeesActivityService employeesActivityService, IMemoryCache cache)
        {
            _repository = repository;
            _employeesActivityService = employeesActivityService;
            _cache = cache;
        }

        public async Task<List<ResponseDeliveryVariantModel>> GetDeliveryVariantsAsync()
        {
            var variants = await _repository.GetDeliveryVariantsAsync();

            return variants.Select(d => new ResponseDeliveryVariantModel
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                Description = d.Description,
                IsActive = d.IsActive,
                ViewOrder = d.ViewOrder
            }).ToList();
        }

        public async Task<string> AddDeliveryVariantAsync(AddDeliveryVariantModel model, int initiatorId)
        {
            var codeExists = await _repository.DeliveryVariantCodeExistsAsync(model.Code);

            if (codeExists)
                return "code_taken";

            await _repository.AddDeliveryVariantAsync(new DeliveryVariantEntity
            {
                Code = model.Code,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) варіант доставки {model.Name}.");
            _cache.Remove(CacheKeys.DeliveryVariants);
            return "ok";
        }

        public async Task<string> UpdateDeliveryVariantAsync(int id, UpdateDeliveryVariantModel model, int initiatorId)
        {
            var variant = await _repository.GetDeliveryVariantByIdAsync(id);

            if (variant == null)
                return "not_found";

            var codeExists = await _repository.DeliveryVariantCodeExistsAsync(model.Code);

            if (codeExists && variant.Code != model.Code)
                return "code_taken";

            var before = JsonSerializer.Serialize(new { variant.Code, variant.Name, variant.Description, variant.IsActive, variant.ViewOrder });

            variant.Code = model.Code;
            variant.Name = model.Name;
            variant.Description = model.Description;
            variant.IsActive = model.IsActive;
            variant.ViewOrder = model.ViewOrder;

            await _repository.UpdateDeliveryVariantAsync(variant);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.Code, model.Name, model.Description, model.IsActive, model.ViewOrder });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) варіант доставки {variant.Name}.", before, after);
            _cache.Remove(CacheKeys.DeliveryVariants);
            return "ok";
        }

        public async Task<string> DeleteDeliveryVariantAsync(int id, int initiatorId)
        {
            var variant = await _repository.GetDeliveryVariantByIdAsync(id);

            if (variant == null)
                return "not_found";

            await _repository.DeleteDeliveryVariantAsync(variant);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) варіант доставки {variant.Name}.");
            _cache.Remove(CacheKeys.DeliveryVariants);
            return "ok";
        }

        public async Task<List<ResponsePaymentVariantModel>> GetPaymentVariantsAsync()
        {
            var variants = await _repository.GetPaymentVariantsAsync();

            return variants.Select(p => new ResponsePaymentVariantModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                IsActive = p.IsActive,
                ViewOrder = p.ViewOrder
            }).ToList();
        }

        public async Task<string> AddPaymentVariantAsync(AddPaymentVariantModel model, int initiatorId)
        {
            var codeExists = await _repository.PaymentVariantCodeExistsAsync(model.Code);

            if (codeExists)
                return "code_taken";

            await _repository.AddPaymentVariantAsync(new PaymentVariantEntity
            {
                Code = model.Code,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) варіант оплати {model.Name}.");
            _cache.Remove(CacheKeys.PaymentVariants);
            return "ok";
        }

        public async Task<string> UpdatePaymentVariantAsync(int id, UpdatePaymentVariantModel model, int initiatorId)
        {
            var variant = await _repository.GetPaymentVariantByIdAsync(id);

            if (variant == null)
                return "not_found";

            var codeExists = await _repository.PaymentVariantCodeExistsAsync(model.Code);

            if (codeExists && variant.Code != model.Code)
                return "code_taken";

            var before = JsonSerializer.Serialize(new { variant.Code, variant.Name, variant.Description, variant.IsActive, variant.ViewOrder });

            variant.Code = model.Code;
            variant.Name = model.Name;
            variant.Description = model.Description;
            variant.IsActive = model.IsActive;
            variant.ViewOrder = model.ViewOrder;
            await _repository.UpdatePaymentVariantAsync(variant);

            var after = JsonSerializer.Serialize(new { model.Code, model.Name, model.Description, model.IsActive, model.ViewOrder });
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) варіант оплати {variant.Name}.", before, after);
            _cache.Remove(CacheKeys.PaymentVariants);
            return "ok";
        }

        public async Task<string> DeletePaymentVariantAsync(int id, int initiatorId)
        {
            var variant = await _repository.GetPaymentVariantByIdAsync(id);

            if (variant == null)
                return "not_found";

            await _repository.DeletePaymentVariantAsync(variant);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) варіант оплати {variant.Name}.");
            _cache.Remove(CacheKeys.PaymentVariants);
            return "ok";
        }

        public async Task ReorderDeliveryVariantsAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetDeliveryVariantsOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);

            await _repository.ReorderDeliveryVariantsAsync(items);
            _cache.Remove(CacheKeys.DeliveryVariants);

            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок варіантів доставки.", before, after);
        }

        public async Task ReorderPaymentVariantsAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetPaymentVariantsOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);

            await _repository.ReorderPaymentVariantsAsync(items);
            _cache.Remove(CacheKeys.PaymentVariants);

            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок варіантів оплати.", before, after);
        }
    }
}