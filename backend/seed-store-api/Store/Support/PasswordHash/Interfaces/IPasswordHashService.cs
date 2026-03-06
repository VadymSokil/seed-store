namespace seed_store_api.Store.Support.PasswordHash.Interfaces
{
    public interface IPasswordHashService
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
