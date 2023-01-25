using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IWalletVotePowerService
    {
        Task<GameUser?> GetWalletVotePowerByAddress(string walletAddress);
        Task<List<GameUserVoteDto>> GetWalletsVotePower();
    }
}
