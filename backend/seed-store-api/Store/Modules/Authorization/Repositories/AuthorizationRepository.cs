using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Context;
using seed_store_api.Database.Entities.Store.Modules.Account;
using seed_store_api.Store.Modules.Authorization.Interfaces;
using seed_store_api.Store.Modules.Authorization.Models;

namespace seed_store_api.Store.Modules.Authorization.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly AppDbContext _context;

        public AuthorizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AccountExistsAsync(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task<bool> PendingAccountExistsAsync(string email)
        {
            return await _context.PendingAccounts.AnyAsync(p => p.Email == email);
        }

        public async Task AddPendingAccountAsync(PendingAccountEntity entity)
        {
            await _context.PendingAccounts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }



        public async Task<PendingAccountEntity?> GetPendingAccountAsync(string email, string code)
        {
            return await _context.PendingAccounts
                .FirstOrDefaultAsync(p => p.Email == email && p.Code == code);
        }

        public async Task<int> AddAccountAsync(AccountEntity entity)
        {
            await _context.Accounts.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeletePendingAccountAsync(PendingAccountEntity entity)
        {
            _context.PendingAccounts.Remove(entity);
            await _context.SaveChangesAsync();
        }



        public async Task<PendingAccountEntity?> GetPendingAccountByEmailAsync(string email)
        {
            return await _context.PendingAccounts
                .FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task UpdatePendingAccountCodeAsync(PendingAccountEntity entity)
        {
            _context.PendingAccounts.Update(entity);
            await _context.SaveChangesAsync();
        }



        public async Task<AccountEntity?> GetAccountByEmailAsync(string email)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task AddRefreshTokenAsync(RefreshTokenEntity entity)
        {
            await _context.RefreshTokens.AddAsync(entity);
            await _context.SaveChangesAsync();
        }



        public async Task<bool> AccountExistsByEmailAsync(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task UpsertPasswordResetRequestAsync(PasswordResetRequestEntity entity)
        {
            var existing = await _context.PasswordResetRequests
                .FirstOrDefaultAsync(p => p.AccountId == entity.AccountId);

            if (existing == null)
            {
                await _context.PasswordResetRequests.AddAsync(entity);
            }
            else
            {
                existing.Code = entity.Code;
                existing.Token = entity.Token;
                existing.ExpiresAt = entity.ExpiresAt;
                _context.PasswordResetRequests.Update(existing);
            }

            await _context.SaveChangesAsync();
        }



        public async Task<PasswordResetRequestEntity?> GetPasswordResetRequestAsync(string email, string code)
        {
            return await _context.PasswordResetRequests
                .Join(_context.Accounts,
                    r => r.AccountId,
                    a => a.Id,
                    (r, a) => new { Request = r, Account = a })
                .Where(x => x.Account.Email == email && x.Request.Code == code)
                .Select(x => x.Request)
                .FirstOrDefaultAsync();
        }

        public async Task UpdatePasswordResetTokenAsync(PasswordResetRequestEntity entity)
        {
            _context.PasswordResetRequests.Update(entity);
            await _context.SaveChangesAsync();
        }



        public async Task<PasswordResetRequestEntity?> GetPasswordResetRequestByTokenAsync(string email, string token)
        {
            return await _context.PasswordResetRequests
                .Join(_context.Accounts,
                    r => r.AccountId,
                    a => a.Id,
                    (r, a) => new { Request = r, Account = a })
                .Where(x => x.Account.Email == email && x.Request.Token == token)
                .Select(x => x.Request)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAccountPasswordAsync(int accountId, string passwordHash)
        {
            await _context.Accounts
                .Where(a => a.Id == accountId)
                .ExecuteUpdateAsync(a => a.SetProperty(x => x.PasswordHash, passwordHash));
        }

        public async Task DeletePasswordResetRequestAsync(PasswordResetRequestEntity entity)
        {
            _context.PasswordResetRequests.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetRequestEntity?> GetPasswordResetRequestByEmailAsync(string email)
        {
            return await _context.PasswordResetRequests
                .Join(_context.Accounts,
                    r => r.AccountId,
                    a => a.Id,
                    (r, a) => new { Request = r, Account = a })
                .Where(x => x.Account.Email == email)
                .Select(x => x.Request)
                .FirstOrDefaultAsync();
        }



        public async Task<RefreshTokenEntity?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task DeleteRefreshTokenAsync(RefreshTokenEntity entity)
        {
            _context.RefreshTokens.Remove(entity);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteRefreshTokenByAccountIdAsync(int accountId)
        {
            await _context.RefreshTokens
                .Where(r => r.AccountId == accountId)
                .ExecuteDeleteAsync();
        }
    }
}