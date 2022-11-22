using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;

namespace Mayhem.Bl.Services.Interfaces
{
    public interface IWalletAuthenticationService
    {
        Task<AuthorizationResponse> GetAuthorizedWalletAsync(string ticket);
    }
}
