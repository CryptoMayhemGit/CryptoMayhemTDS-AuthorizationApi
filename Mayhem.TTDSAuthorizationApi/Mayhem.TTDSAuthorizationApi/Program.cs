using AspNetCoreRateLimit;
using Mayhem.ApplicationSetup;
using Mayhem.Configuration;
using Mayhem.TTDSAuthorizationApi.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.AzureAppServices;

string CorsPolicy = nameof(CorsPolicy);

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.RealIpHeader = "X-Real-IP";
    options.ClientIdHeader = "X-ClientId";
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "60s",
            Limit = 10
        }
    };
});
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy,
        builder => builder.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
    .Build());
});

builder.Services.AddMvc(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Configuration.ConfigureKeyVault();
string sqlConnectionString = builder.Configuration["SqlConnectionString"];
string zealyApiKey = builder.Configuration["ZealyApiKey"];
string purchasePriceMultiplier = builder.Configuration["PurchasePriceMultiplier"];

builder.Services.AddSingleton(new MayhemConfiguration(sqlConnectionString, zealyApiKey, purchasePriceMultiplier));

builder.Services.AddMayhemContext(sqlConnectionString);
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddServices();
builder.Services.AddRepository();
builder.Services.AddValidators();
builder.Services.AddMayhemHttpClient();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    loggingBuilder.AddAzureWebAppDiagnostics(); // Dodaj logowanie do Azure
});


var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseIpRateLimiting();
app.UseHttpsRedirection();

app.UseCors(CorsPolicy);
app.UseAuthorization();

app.MapControllers();

app.Run();
