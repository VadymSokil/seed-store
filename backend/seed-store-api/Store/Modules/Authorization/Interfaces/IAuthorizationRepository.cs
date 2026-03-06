using seed_store_api.Database.Entities.Store.Modules.Account;

namespace seed_store_api.Store.Modules.Authorization.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<bool> AccountExistsAsync(string email);
        Task<bool> PendingAccountExistsAsync(string email);
        Task AddPendingAccountAsync(PendingAccountEntity entity);

        Task<PendingAccountEntity?> GetPendingAccountAsync(string email, string code);
        Task<int> AddAccountAsync(AccountEntity entity);
        Task DeletePendingAccountAsync(PendingAccountEntity entity);

        Task<PendingAccountEntity?> GetPendingAccountByEmailAsync(string email);
        Task UpdatePendingAccountCodeAsync(PendingAccountEntity entity);

        Task<AccountEntity?> GetAccountByEmailAsync(string email);
        Task AddRefreshTokenAsync(RefreshTokenEntity entity);

        Task<bool> AccountExistsByEmailAsync(string email);
        Task UpsertPasswordResetRequestAsync(PasswordResetRequestEntity entity);

        Task<PasswordResetRequestEntity?> GetPasswordResetRequestAsync(string email, string code);
        Task UpdatePasswordResetTokenAsync(PasswordResetRequestEntity entity);

        Task<PasswordResetRequestEntity?> GetPasswordResetRequestByTokenAsync(string email, string token);
        Task UpdateAccountPasswordAsync(int accountId, string passwordHash);
        Task DeletePasswordResetRequestAsync(PasswordResetRequestEntity entity);

        Task<PasswordResetRequestEntity?> GetPasswordResetRequestByEmailAsync(string email);

        Task<RefreshTokenEntity?> GetRefreshTokenAsync(string token);
        Task DeleteRefreshTokenAsync(RefreshTokenEntity entity);

        Task DeleteRefreshTokenByAccountIdAsync(int accountId);
    }
}
