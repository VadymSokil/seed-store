using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.Roles.Interfaces;
using SeedStore.Admin.Roles.Models;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Admin.Roles;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Roles.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly AppDbContext _context;

        public RolesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleEntity>> GetRolesAsync()
        {
            return await _context.Roles.OrderBy(r => r.ViewOrder).ToListAsync();
        }

        public async Task<RoleEntity?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<bool> RoleCodeExistsAsync(string code)
        {
            return await _context.Roles.AnyAsync(r => r.Code == code);
        }

        public async Task AddRoleAsync(RoleEntity role)
        {
            role.ViewOrder = await GetNextRoleViewOrderAsync();
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(RoleEntity role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(RoleEntity role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task UpdateEmployeeAsync(EmployeeEntity employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PermissionEntity>> GetRolePermissionsAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Join(_context.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
                .ToListAsync();
        }

        public async Task<List<RolePermissionEntity>> GetRolePermissionEntitiesAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();
        }

        public async Task DeleteRolePermissionsAsync(List<RolePermissionEntity> permissions)
        {
            _context.RolePermissions.RemoveRange(permissions);
            await _context.SaveChangesAsync();
        }

        public async Task AddRolePermissionsAsync(List<RolePermissionEntity> permissions)
        {
            var nextViewOrder = await GetNextRolePermissionViewOrderAsync();
            foreach (var permission in permissions)
            {
                permission.ViewOrder = nextViewOrder++;
            }
            await _context.RolePermissions.AddRangeAsync(permissions);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PermissionEntity>> GetPermissionsAsync()
        {
            return await _context.Permissions.OrderBy(p => p.ViewOrder).ToListAsync();
        }

        public async Task<PermissionEntity?> GetPermissionByIdAsync(int id)
        {
            return await _context.Permissions.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> PermissionCodeExistsAsync(string code)
        {
            return await _context.Permissions.AnyAsync(p => p.Code == code);
        }

        public async Task AddPermissionAsync(PermissionEntity entity)
        {
            entity.ViewOrder = await GetNextPermissionViewOrderAsync();
            await _context.Permissions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(PermissionEntity entity)
        {
            _context.Permissions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(PermissionEntity entity)
        {
            _context.Permissions.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetNextRoleViewOrderAsync()
        {
            return (await _context.Roles.MaxAsync(c => c.ViewOrder) ?? 0) + 1;
        }

        public async Task<int> GetNextPermissionViewOrderAsync()
        {
            return (await _context.Permissions.MaxAsync(c => c.ViewOrder) ?? 0) + 1;
        }

        public async Task<int> GetNextRolePermissionViewOrderAsync()
        {
            return (await _context.RolePermissions.MaxAsync(c => c.ViewOrder) ?? 0) + 1;
        }

        public async Task ReorderRolesAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.Roles
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderPermissionsAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.Permissions
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderRolePermissionsAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.RolePermissions
                    .Where(c => c.RoleId == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task<List<ReorderItemModel>> GetRolesOrderAsync()
        {
            return await _context.Roles
                .OrderBy(r => r.ViewOrder)
                .Select(r => new ReorderItemModel { Id = r.Id, ViewOrder = r.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<ReorderItemModel>> GetPermissionsOrderAsync()
        {
            return await _context.Permissions
                .OrderBy(p => p.ViewOrder)
                .Select(p => new ReorderItemModel { Id = p.Id, ViewOrder = p.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<RolePermissionOrderModel>> GetRolePermissionsOrderAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .OrderBy(rp => rp.ViewOrder)
                .Join(_context.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => new RolePermissionOrderModel
                {
                    PermissionId = rp.PermissionId,
                    Name = p.Name,
                    ViewOrder = rp.ViewOrder ?? 0
                })
                .ToListAsync();
        }
    }
}