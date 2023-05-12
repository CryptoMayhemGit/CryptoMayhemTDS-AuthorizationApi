using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Response;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;

namespace Mayhem.Bl.Services.Implementations
{
    public class InvestorService : IInvestorService
    {
        private readonly ILogger<WalletAuthenticationService> logger;
        private readonly IGameUserRepository gameUserRepository;

        public InvestorService(ILogger<WalletAuthenticationService> logger,
                                           IGameUserRepository gameUserRepository)
        {
            this.logger = logger;
            this.gameUserRepository = gameUserRepository;
        }

        public async Task<GetInvestorStatusResponse> CheckIsExistAsync(string wallet)
        {
            if (string.IsNullOrEmpty(wallet))
            {
                AddErrorBadRequest();
            }

            return new GetInvestorStatusResponse() { isInvestor = await gameUserRepository.CheckWallet(wallet) };
        }

        private void AddErrorBadRequest()//TODO to separate class.
        {
            string errorMessage = $"Bad request.";
            logger.LogInformation(errorMessage);
            throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
        }
    }
}
