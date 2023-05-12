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
        private readonly IWalletAuthenticationInGameService walletAuthenticationInGameService;

        public AuthorizationController(IWalletAuthenticationService walletAuthenticationService, IWalletAuthenticationInGameService walletAuthenticationInGameService)
        {
            this.walletAuthenticationService = walletAuthenticationService;
            this.walletAuthenticationInGameService = walletAuthenticationInGameService;
        }

        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(typeof(AuthorizationResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTicket([FromBody] AuthorizationRequest request)
        {
            AuthorizationResponse? response = await walletAuthenticationService.GetAuthorizedWalletAsync(request.Ticket);
            return CreatedAtAction(nameof(GetTicket), response);
        }

        [Route("LoginToGame")]
        [HttpPost]
        [ProducesResponseType(typeof(AuthorizationResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetTicketInGame([FromBody] AuthorizationRequest request)
        {
            AuthorizationResponse? response = await walletAuthenticationInGameService.GetAuthorizedWalletInGameAsync(request.Ticket);
            return CreatedAtAction(nameof(GetTicket), response);
        }
    }
}