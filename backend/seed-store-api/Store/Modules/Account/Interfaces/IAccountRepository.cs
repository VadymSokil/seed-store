using seed_store_api.Database.Entities.Store.Modules.Account;

namespace seed_store_api.Store.Modules.Account.Interfaces
{
    public interface IAccountRepository
    {
        Task<AccountEntity?> GetAccountByIdAsync(int id);
        Task UpdateAccountNameAsync(int accountId, string firstName, string lastName, string? middleName);

        Task<bool> EmailExistsAsync(string email);
        Task UpsertEmailChangeRequestAsync(EmailChangeRequestEntity entity);

        Task<EmailChangeRequestEntity?> GetEmailChangeRequestAsync(int accountId, string newEmail, string code);
        Task UpdateAccountEmailAsync(int accountId, string newEmail);
        Task DeleteEmailChangeRequestAsync(EmailChangeRequestEntity entity);

        Task UpdateAccountPhoneAsync(int accountId, string? phoneNumber);

        Task UpdateAccountPasswordAsync(int accountId, string passwordHash);

        Task DeleteAccountAsync(int accountId);
    }
}
