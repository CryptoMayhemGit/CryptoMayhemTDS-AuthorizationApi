using Mayhem.Bl.Services.Interfaces;
using Mayhem.Configuration;
using Mayhem.Dal.Repositories.Interfaces;

namespace Mayhem.Bl.Services.Implementations
{
    public class VestingService : IVestingService
    {
        private readonly IGameUserRepository gameUserRepository;
        private readonly MayhemConfiguration mayhemConfiguration;

        public VestingService(IGameUserRepository gameUserRepository, MayhemConfiguration mayhemConfiguration)
        {
            this.gameUserRepository = gameUserRepository;
            this.mayhemConfiguration = mayhemConfiguration;
        }

        public async Task<int> GetBlockedTokens(string walletAddress)
        {
            int gameUserInvestment = await gameUserRepository.GetGameUserInvestment(walletAddress);
            return gameUserInvestment * mayhemConfiguration.PurchasePriceMultiplier;
        }
    }
}
