using AutoMapper;
using Mayhem.Dal.Context;
using Mayhem.Dal.Repositories.Interfaces;
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
    }
}
