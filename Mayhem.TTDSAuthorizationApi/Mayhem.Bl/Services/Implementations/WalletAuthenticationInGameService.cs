using FluentValidation;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;
using Mayhem.Dal.Repositories.Interfaces;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Mayhem.Bl.Services.Implementations
{
    public class WalletAuthenticationInGameService : IWalletAuthenticationInGameService
    {
        private readonly IValidator<AuthorizationDecodedRequest> authorizationRequestValidator;
        private readonly ILogger<WalletAuthenticationService> logger;
        private readonly ITicketEndoceService ticketEndoceService;
        private readonly IGameUserRepository gameUserRepository;
        private readonly HttpClient httpClient;

        public WalletAuthenticationInGameService(ILogger<WalletAuthenticationService> logger,
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

        public async Task<AuthorizationResponse> GetAuthorizedWalletInGameAsync(string ticket)
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
                try
                {
                    result = await gameUserRepository.CheckWallet(authorizationDecodedRequest.signedData.Wallet);
                }catch (Exception ex)
                {
                    throw;
                }
            }

            if (result == false)
            {
                AddErrorAccessDenied();
            }

            return new AuthorizationResponse() { Wallet = authorizationDecodedRequest.signedData.Wallet };
        }

        private async Task ValidateRequest(AuthorizationDecodedRequest authorizationDecodedRequest)
        {
            string? handleWallet = string.Empty;
            string? accessToken = string.Empty;

            if (!string.IsNullOrEmpty(authorizationDecodedRequest.signedData.Handle))
            {
                try
                {
                    accessToken = await GetCyberConnectAccesTokenAsync(authorizationDecodedRequest.signedData.Wallet, authorizationDecodedRequest.signedFreshData.Signature);
                    handleWallet = await GetProfileByHandle(authorizationDecodedRequest.signedData.Handle);
                    if (string.IsNullOrEmpty(handleWallet) && handleWallet?.ToLower() != authorizationDecodedRequest.signedData.Wallet)
                    {
                        AddErrorAccessDenied();
                    }
                }catch (Exception ex)
                {
                    throw;
                }
            }

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

        private async Task<string> GetProfileByHandle(string handle)
        {
            var mutation = @"query getProfileByHandle($handle: String!){
                            profileByHandle(handle: $handle) {
                            metadataInfo {
                                    avatar
                                    bio
                                }
                                owner {
                                    address
                                }
                                isPrimary
                            }
                            }";

            var variables = new
            {
                handle
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.cyberconnect.dev/")
            {
                Content = new StringContent(JsonSerializer.Serialize(new { query = mutation, variables }), Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(content))
            {
                var result = JsonSerializer.Deserialize<ProfileByHandleDataResponse>(content);
                if (result.data.profileByHandle == null) return string.Empty;
                return result.data.profileByHandle.owner.address;
            }

            return string.Empty;
        }
    }
}
