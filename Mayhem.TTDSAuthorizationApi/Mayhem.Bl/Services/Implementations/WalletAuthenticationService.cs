using FluentValidation;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;

namespace Mayhem.Bl.Services.Implementations
{
    public class WalletAuthenticationService : IWalletAuthenticationService
    {
        private readonly IValidator<AuthorizationDecodedRequest> authorizationRequestValidator;
        private readonly ILogger<WalletAuthenticationService> logger;
        private readonly ITicketEndoceService ticketEndoceService;

        public WalletAuthenticationService(ILogger<WalletAuthenticationService> logger,
                                           IValidator<AuthorizationDecodedRequest> authorizationRequestValidator,
                                           ITicketEndoceService ticketEndoceService)
        {
            this.authorizationRequestValidator = authorizationRequestValidator;
            this.logger = logger;
            this.ticketEndoceService = ticketEndoceService;
        }

        public async Task<AuthorizationResponse> GetAuthorizedWalletAsync(string ticket)
        {
            if (string.IsNullOrEmpty(ticket))
            {
                AddErrorBadRequest();
            }

            AuthorizationDecodedRequest authorizationDecodedRequest = ticketEndoceService.DecodeTicket(ticket);
            await ValidateRequest(authorizationDecodedRequest);
            return new AuthorizationResponse() { Wallet = authorizationDecodedRequest.signedData.Wallet };
        }

        private async Task ValidateRequest(AuthorizationDecodedRequest authorizationDecodedRequest)
        {
            var validationResult = await authorizationRequestValidator.ValidateAsync(authorizationDecodedRequest);

            if (validationResult.IsValid == false)
            {
                string firstErrorCode = validationResult.Errors.First().ErrorCode;

                if (firstErrorCode == "ACCESS_DENIED")
                {
                    AddErrorAccessDenied();
                }
                else
                {
                    AddErrorBadRequest();
                }
            }
        }

        private void AddErrorAccessDenied()//TODO to separate class.
        {
            string errorMessage = $"Access denied.";
            logger.LogInformation(errorMessage);
            throw new NotFoundException(new ValidationMessage("ACCESS_DENIED", errorMessage));
        }

        private void AddErrorBadRequest()//TODO to separate class.
        {
            string errorMessage = $"Bad request.";
            logger.LogInformation(errorMessage);
            throw new NotFoundException(new ValidationMessage("BAD_REQUEST", errorMessage));
        }

    }
}
