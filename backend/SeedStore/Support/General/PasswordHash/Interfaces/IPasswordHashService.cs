namespace SeedStore.Support.General.PasswordHash.Interfaces
{
    public interface IPasswordHashService
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
