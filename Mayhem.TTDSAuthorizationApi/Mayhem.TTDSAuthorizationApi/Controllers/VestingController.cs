using Mayhem.Bl.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mayhem.TTDSAuthorizationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VestingController : ControllerBase
    {
        private readonly IVestingService vestingService;

        public VestingController(
            IVestingService vestingService
            )
        {
            this.vestingService = vestingService;
        }

        [Route("GetBlockedTokens")]
        [HttpGet]
        public async Task<ActionResult> GetBlockedTokens(string walletAddress)
        {
            int vestingTokens = await vestingService.GetBlockedTokens(walletAddress);
            return CreatedAtAction(nameof(GetBlockedTokens), new { vestingTokens = vestingTokens });
        }
    }
}
