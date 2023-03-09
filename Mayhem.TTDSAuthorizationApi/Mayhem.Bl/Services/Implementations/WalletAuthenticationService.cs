using FluentValidation;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace Mayhem.Bl.Services.Implementations
{
    public class WalletAuthenticationService : IWalletAuthenticationService
    {
        private readonly IValidator<AuthorizationDecodedRequest> authorizationRequestValidator;
        private readonly ILogger<WalletAuthenticationService> logger;
        private readonly ITicketEndoceService ticketEndoceService;
        private readonly IGameUserRepository gameUserRepository;
        private readonly HttpClient httpClient;

        public WalletAuthenticationService(ILogger<WalletAuthenticationService> logger,
                                           IValidator<AuthorizationDecodedRequest> authorizationRequestValidator,
                                           ITicketEndoceService ticketEndoceService,
                                           IGameUserRepository gameUserRepository,
                                           HttpClient httpClient)
        {
            this.authorizationRequestValidator = authorizationRequestValidator;
            this.logger = logger;
            this.ticketEndoceService = ticketEndoceService;
            this.gameUserRepository = gameUserRepository;
            this.httpClient = httpClient;
        }

        public async Task<AuthorizationResponse> GetAuthorizedWalletAsync(string ticket)
        {
            bool result = true;
            if (string.IsNullOrEmpty(ticket))
            {
                AddErrorBadRequest();
            }

            AuthorizationDecodedRequest authorizationDecodedRequest = ticketEndoceService.DecodeTicket(ticket);
            await ValidateRequest(authorizationDecodedRequest);

            if (authorizationDecodedRequest.isCyberConnect == false)
            {
                result = await gameUserRepository.CheckWallet(authorizationDecodedRequest.signedData.Wallet);
            }

            if (result == false)
            {
                AddErrorAccessDenied();
            }

            return new AuthorizationResponse() { Wallet = authorizationDecodedRequest.signedData.Wallet };
        }

        private async Task ValidateRequest(AuthorizationDecodedRequest authorizationDecodedRequest)
        {
            var accessToken = await GetCyberConnectAccesTokenAsync(authorizationDecodedRequest.signedData.Wallet, authorizationDecodedRequest.signedFreshData.Signature);

            if (string.IsNullOrEmpty(accessToken))
            {
                authorizationDecodedRequest.isCyberConnect = false;
            } else 
            {
                authorizationDecodedRequest.isCyberConnect = true;
            } 

            var validationResult = await authorizationRequestValidator.ValidateAsync(authorizationDecodedRequest);

            if (validationResult.IsValid == false && string.IsNullOrEmpty(accessToken))
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

        private async Task<string> GetCyberConnectAccesTokenAsync(string address, string signature)
        {
            var mutation = @"mutation loginVerify($domain:String!,$address:AddressEVM!,$signature:String!) {
                          loginVerify(input:{
                            domain:$domain,
                            address:$address,
                            signature:$signature
                          }){
                            accessToken
                          }
                        }";

            var variables = new
            {
                domain = "cryptomayhem.io",
                address,
                signature
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.cyberconnect.dev/")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { query = mutation, variables }), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(content))
            {
                var result = JsonSerializer.Deserialize<CcLoginResponse>(content);
                if (result.data == null) return string.Empty;
                return result.data.loginVerify.accessToken.ToString();
            }

            return string.Empty;
        }
    }
}
