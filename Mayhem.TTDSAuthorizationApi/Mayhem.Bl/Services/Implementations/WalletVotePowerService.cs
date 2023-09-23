using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;

namespace Mayhem.Bl.Services.Implementations
{
    public class WalletVotePowerService : IWalletVotePowerService
    {
        private readonly IGameUserRepository gameUserRepository;
        public WalletVotePowerService(
            IGameUserRepository gameUserRepository)
        {
            this.gameUserRepository = gameUserRepository;
        }

        public async Task<GameUser?> GetWalletVotePowerByAddressAsync(string walletAddress)
        {
            return await gameUserRepository.GetGameUserByWalletAddress(walletAddress);
        }

        public async Task<List<GameUserVoteDto>> GetWalletsVotePowerAsync()
        {
            List<GameUser> gameUsersWithoutDuplicates = new();
            List<GameUser> gameUsers = await this.gameUserRepository.GetGameUsers();

            return gameUsers.GroupBy(x => x.Wallet)
                .Select(g => new GameUserVoteDto
                {
                    Id = g.OrderByDescending(x => x.VoteCategory.VotePower).First().Id,
                    Wallet = g.Key,
                    VotePower = g.Max(x => x.VoteCategory.VotePower)
                }).ToList();
        }
    }
}
