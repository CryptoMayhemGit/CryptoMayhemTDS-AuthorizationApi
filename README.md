# Mayhem TTDS Authorization API

Mayhem TTDS Authorization API is a .NET 7 web API that provides authentication and investor status services. The API allows users to authenticate their wallets and check their investor status.

## Table of Contents

- [Requirements](#requirements)
- [Setup](#setup)
- [Configuration](#configuration)
- [Usage](#usage)
- [Endpoints](#endpoints)
- [Logging](#logging)
- [Rate Limiting](#rate-limiting)
- [Contributing](#contributing)
- [License](#license)

## Requirements

- .NET 7 SDK
- Azure Key Vault
- SQL Server

## Setup

1. Clone the repository:
   git clone https://github.com/your-repo/mayhem-ttds-authorization-api.git cd mayhem-ttds-authorization-api
   
2. Restore the dependencies:
   dotnet restore
   
3. Build the project:
	dotnet build

4. Run the project:
dotnet run --project Mayhem.TTDSAuthorizationApi


## Configuration

The application requires the following configuration settings in `appsettings.json` or environment variables:

- `SqlConnectionString`: The connection string to the SQL Server database.
- `ZealyApiKey`: The API key for the Zealy service.
- `PurchasePriceMultiplier`: The purchase price multiplier.

Additionally, configure Azure Key Vault by setting the environment variable `TDSAuthorizationApiKeyVaultEndpoint` with the Key Vault endpoint.

## Usage

The API provides endpoints for wallet authentication and checking investor status. You can use tools like Postman or cURL to interact with the API.

## Endpoints

### Login

- **URL**: `/api/Authorization/Login`
- **Method**: `POST`
- **Request Body**:
  { "ticket": "string" }
  - **Response**:
  { "wallet": "string" }
  
### Get Investor Status

- **URL**: `/api/Authorization/GetInvestorStatus`
- **Method**: `GET`
- **Query Parameters**:
  - `wallet`: The wallet address to check.
- **Response**:
  { "isInvestor": true }
  
## Logging

The application uses `ILogger` for logging. Ensure that the environment variable `DIAGNOSTICS_AZUREBLOBCONTAINERSASURL` is set to enable Azure Blob Storage logging.

## Rate Limiting

The application uses `AspNetCoreRateLimit` for rate limiting. The default configuration limits requests to 10 per minute per endpoint.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License. The license belongs to Mayhem Games OÜ.

  