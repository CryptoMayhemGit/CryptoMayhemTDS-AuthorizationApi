using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IWalletAuthenticationInGameService
    {
        Task<AuthorizationResponse?> GetAuthorizedWalletInGameAsync(string ticket);
    }
}
