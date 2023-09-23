using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Tables;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IWalletVotePowerService
    {
        Task<GameUser?> GetWalletVotePowerByAddressAsync(string walletAddress);
        Task<List<GameUserVoteDto>> GetWalletsVotePowerAsync();
    }
}
