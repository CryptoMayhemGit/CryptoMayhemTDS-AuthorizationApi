using AutoMapper;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Tables;
using Microsoft.AspNetCore.Mvc;

namespace Mayhem.TTDSAuthorizationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotePowerController : ControllerBase
    {
        private readonly IWalletVotePowerService walletVotePowerService;
        private readonly IMapper mapper;

        public VotePowerController(
            IWalletVotePowerService walletVotePowerService,
            IMapper mapper
            )
        {
            this.walletVotePowerService = walletVotePowerService;
            this.mapper = mapper;
        }

        [Route("wallet/{walletAddress}")]
        [HttpGet]
        public async Task<ActionResult> GetWalletVotePowerByWalletAddress(string walletAddress)
        {
            GameUser? gameUser = await walletVotePowerService
                .GetWalletVotePowerByAddressAsync(walletAddress);
            return CreatedAtAction(nameof(GetWalletVotePowerByWalletAddress), mapper.Map<GameUserVoteDto>(gameUser));
        }

        [Route("wallets")]
        [HttpGet]
        public async Task<ActionResult> GetWalletsVotePower()
        {
            List<GameUserVoteDto> gameUsersVoteDto = await walletVotePowerService.GetWalletsVotePowerAsync();
            return CreatedAtAction(nameof(GetWalletsVotePower), gameUsersVoteDto);
        }
    }
}
