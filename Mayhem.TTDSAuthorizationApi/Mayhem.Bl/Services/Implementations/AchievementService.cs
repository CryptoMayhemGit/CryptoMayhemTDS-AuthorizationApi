using FluentValidation;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;

namespace Mayhem.Bl.Services.Implementations
{
    public class AchievementService : IAchievementService
    {
        private readonly IZealyService zealyService;
        private readonly ITicketEndoceService ticketEndoceService;
        private readonly ILogger<AchievementService> logger;
        private readonly IValidator<AuthorizationDecodedRequest> authorizationRequestValidator;

        public AchievementService(IZealyService zealyService, ITicketEndoceService ticketEndoceService, ILogger<AchievementService> logger, IValidator<AuthorizationDecodedRequest> authorizationRequestValidator)
        {
            this.zealyService = zealyService;
            this.ticketEndoceService = ticketEndoceService;
            this.authorizationRequestValidator = authorizationRequestValidator;
            this.logger = logger;
        }

        public async Task<GetLevelResponse> GetLevelAsync(GetLevelRequest request)
        {
            if (string.IsNullOrEmpty(request.Ticket))
            {
                AddErrorBadRequest();
            }

            AuthorizationDecodedRequest authorizationDecodedRequest = ticketEndoceService.DecodeTicket(request.Ticket);
            await ValidateRequest(authorizationDecodedRequest);


            return await zealyService.GetLevelAsync(authorizationDecodedRequest.signedData.Wallet);
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
