using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Context;
using SeedStore.Support.Admin.PermissionsHandling.Interfaces;

namespace SeedStore.Support.Admin.PermissionsHandling.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetRolePermissionCodesByRoleCodeAsync(string roleCode)
        {
            return await _context.Roles
                .Where(r => r.Code == roleCode)
                .Join(_context.RolePermissions, r => r.Id, rp => rp.RoleId, (r, rp) => rp)
                .Join(_context.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p.Code)
                .ToListAsync();
        }
    }
}