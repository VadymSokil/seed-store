using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Context;
using seed_store_api.Database.Entities.Store.Modules.Account;
using seed_store_api.Store.Modules.Account.Interfaces;

namespace seed_store_api.Store.Modules.Account.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccountEntity?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAccountNameAsync(int accountId, string firstName, string lastName, string? middleName)
        {
            await _context.Accounts
                .Where(a => a.Id == accountId)
                .ExecuteUpdateAsync(a => a
                    .SetProperty(x => x.FirstName, firstName)
                    .SetProperty(x => x.LastName, lastName)
                    .SetProperty(x => x.MiddleName, middleName));
        }



        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task UpsertEmailChangeRequestAsync(EmailChangeRequestEntity entity)
        {
            var existing = await _context.EmailChangeRequests
                .FirstOrDefaultAsync(e => e.AccountId == entity.AccountId);

            if (existing == null)
            {
                await _context.EmailChangeRequests.AddAsync(entity);
            }
            else
            {
                existing.NewEmail = entity.NewEmail;
                existing.Code = entity.Code;
                existing.ExpiresAt = entity.ExpiresAt;
                _context.EmailChangeRequests.Update(existing);
            }

            await _context.SaveChangesAsync();
        }



        public async Task<EmailChangeRequestEntity?> GetEmailChangeRequestAsync(int accountId, string newEmail, string code)
        {
            return await _context.EmailChangeRequests
                .FirstOrDefaultAsync(e => e.AccountId == accountId && e.NewEmail == newEmail && e.Code == code);
        }

        public async Task UpdateAccountEmailAsync(int accountId, string newEmail)
        {
            await _context.Accounts
                .Where(a => a.Id == accountId)
                .ExecuteUpdateAsync(a => a.SetProperty(x => x.Email, newEmail));
        }

        public async Task DeleteEmailChangeRequestAsync(EmailChangeRequestEntity entity)
        {
            _context.EmailChangeRequests.Remove(entity);
            await _context.SaveChangesAsync();
        }



        public async Task UpdateAccountPhoneAsync(int accountId, string? phoneNumber)
        {
            await _context.Accounts
                .Where(a => a.Id == accountId)
                .ExecuteUpdateAsync(a => a.SetProperty(x => x.PhoneNumber, phoneNumber));
        }



        public async Task UpdateAccountPasswordAsync(int accountId, string passwordHash)
        {
            await _context.Accounts
                .Where(a => a.Id == accountId)
                .ExecuteUpdateAsync(a => a.SetProperty(x => x.PasswordHash, passwordHash));
        }


        public async Task DeleteAccountAsync(int accountId)
        {
            await _context.Accounts
                .Where(a => a.Id == accountId)
                .ExecuteDeleteAsync();
        }
    }
}