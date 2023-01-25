using Mayhem.Dal.Tables;

namespace Mayhem.Dal.Repositories.Interfaces
{
    public interface IGameUserRepository
    {
        Task<bool> CheckWallet(string wallet);
        Task<List<GameUser>> GetGameUsers();
        Task<GameUser?> GetGameUserByWalletAddress(string walletAddress);
    }
}
