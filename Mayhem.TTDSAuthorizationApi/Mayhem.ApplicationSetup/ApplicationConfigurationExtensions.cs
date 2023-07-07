using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using HotChocolate.Execution;
using Mayhem.Bl.Services.Implementations;
using Mayhem.Bl.Services.Interfaces;
using Mayhem.Bl.Validators;
using Mayhem.Dal.Context;
using Mayhem.Dal.Dto.Requests;
using Mayhem.Dal.Mappings;
using Mayhem.Dal.Repositories.Implementations;
using Mayhem.Dal.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mayhem.ApplicationSetup
{
    public static class ApplicationConfigurationExtensions
    {
        public static void AddMayhemContext(this IServiceCollection services, string connectionString)
        {
            services
                .AddDbContext<MayhemDataContext>
                (
                    options => options.UseSqlServer(connectionString)
                );
        }

        public static void ConfigureKeyVault(this IConfigurationBuilder configuration)
        {
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            string keyVaultEndpoint = Environment.GetEnvironmentVariable("TDSAuthorizationApiKeyVaultEndpoint");

            if (isDevelopment)
            {
                configuration.AddAzureKeyVault(
                    new Uri(keyVaultEndpoint),
                    new DefaultAzureCredential(new DefaultAzureCredentialOptions
                    {
                        ManagedIdentityClientId = "ef6d1fbb-4f75-4b1b-be94-7cbac1f585c6"
                    }));
            }
            else
            {
                SecretClient secretClient = new(new(keyVaultEndpoint), new DefaultAzureCredential());
                configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
            }
        }

        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(TableDtoMappings));
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IWalletAuthenticationService, WalletAuthenticationService>();
            services.AddScoped<IInvestorService, InvestorService>();
            services.AddScoped<IBlockchainService, BlockchainService>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<ITicketEndoceService, TicketEndoceService>();
            services.AddScoped<IWalletVotePowerService, WalletVotePowerService>();
            services.AddScoped<IZealyService, ZealyService>();
        }

        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IGameUserRepository, GameUserRepository>();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AuthorizationDecodedRequest>, AuthorizationRequestValidator>();
        }

        public static void AddMayhemHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<WalletAuthenticationService>(client =>
            {
                client.BaseAddress = new Uri("https://api.cyberconnect.dev/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}