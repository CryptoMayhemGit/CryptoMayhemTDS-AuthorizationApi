using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Mayhem.TTDSAuthorizationApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IWalletAuthenticationService walletAuthenticationService;
        private readonly IInvestorService investorService;

        public AuthorizationController(IWalletAuthenticationService walletAuthenticationService, IInvestorService walletAuthenticationInGameService)
        {
            this.walletAuthenticationService = walletAuthenticationService;
            this.investorService = walletAuthenticationInGameService;
        }

        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(typeof(AuthorizationResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTicket([FromBody] AuthorizationRequestg request)
        {
            AuthorizationResponse? response = await walletAuthenticationService.GetAuthorizedWalletAsync(request.Ticket);
            return CreatedAtAction(nameof(GetTicket), response);
        }

        [Route("GetInvestorStatus")]
        [HttpGet]
        [ProducesResponseType(typeof(GetInvestorStatusResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetInvestorStatus([FromQuery] GetInvestorStatusRequest request)
        {
            GetInvestorStatusResponse? response = await investorService.CheckIsExistAsync(request.Wallet);
            return CreatedAtAction(nameof(GetTicket), response);
        }
    }
}