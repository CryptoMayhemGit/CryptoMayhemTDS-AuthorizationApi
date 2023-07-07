using Mayhem.Bl.Services.Interfaces;
using Mayhem.Configuration;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Dto.Response;
using Mayhem.Util.Classes;
using Mayhem.Util.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Mayhem.Bl.Services.Implementations
{
    public class ZealyService : IZealyService
    {
        private const string ErrorMessageConnection = "Cannot communicate with zealy api.";
        private const string ErrorMessageFailure = "The query is not correct.";
        private readonly HttpClient httpClient;
        private readonly ILogger<ZealyService> logger;

        public ZealyService(IHttpClientFactory httpClientFactory, ILogger<ZealyService> logger, MayhemConfiguration mayhemConfiguration)
        {
            this.logger = logger;

            httpClient = httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("x-api-key", mayhemConfiguration.ZealyApiKey);
        }

        public async Task<GetLevelResponse> GetLevelAsync(string wallet)
        {
            HttpResponseMessage response;
            try
            {
                response = await httpClient.GetAsync($"https://api.zealy.io/communities/cryptomayhem/users?ethAddress={wallet}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ErrorMessageConnection);
                throw new InternalException(new ValidationMessage("Z01", ErrorMessageConnection));
            }

            if (response.IsSuccessStatusCode)
            {
                string products = await response.Content.ReadAsStringAsync();

                ZealyResponse? zealyResponse = JsonConvert.DeserializeObject<ZealyResponse>(products);

                return new GetLevelResponse() { Level = zealyResponse.Level };
            }
            else
            {
                logger.LogError(ErrorMessageFailure);
                throw new InternalException(new ValidationMessage("Z02", ErrorMessageFailure));
            }
        }
    }
}
