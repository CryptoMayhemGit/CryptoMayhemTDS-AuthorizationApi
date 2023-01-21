using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<GameUser?> GetWalletVotePowerByAddress(string walletAddress)
        {
            return await gameUserRepository.GetGameUserByWalletAddress(walletAddress);
        }

        public async Task<List<GameUserVoteDto>> GetWalletsVotePower()
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
