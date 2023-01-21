using AutoMapper;
using Mayhem.Dal.Context;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Dal.Tables;
using Microsoft.EntityFrameworkCore;

namespace Mayhem.Dal.Repositories.Implementations
{
    public class GameUserRepository : IGameUserRepository
    {
        private readonly MayhemDataContext mayhemDataContext;
        private readonly IMapper mapper;

        public GameUserRepository(MayhemDataContext mayhemDataContext, IMapper mapper)
        {
            this.mayhemDataContext = mayhemDataContext;
            this.mapper = mapper;
        }

        public async Task<bool> CheckWallet(string wallet)
        {
            return await mayhemDataContext
                .GameUsers
                .AsNoTracking()
                .AnyAsync(x => x.Wallet == wallet);
        }

        public async Task<List<GameUser>> GetGameUsers()
        {
            return await mayhemDataContext
                .GameUsers
                .Include(x => x.VoteCategory)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<GameUser?> GetGameUserByWalletAddress(string walletAddress)
        {
            return await mayhemDataContext
                .GameUsers
                .Include(x => x.VoteCategory)
                .Where(x => x.Wallet == walletAddress)
                .SingleOrDefaultAsync();
        }
    }
}
