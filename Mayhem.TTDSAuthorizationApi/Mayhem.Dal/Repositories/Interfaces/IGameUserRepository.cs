namespace Mayhem.Dal.Repositories.Interfaces
{
    public interface IGameUserRepository
    {
        Task<bool> CheckWallet(string wallet);
    }
}
