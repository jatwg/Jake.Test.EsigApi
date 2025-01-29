# E-Signature API

A modern ASP.NET Core Web API for managing electronic signature requests and processing. This solution provides a robust and secure way to handle e-signature workflows in a microservices architecture.

## 🚀 Features

- Send e-signature requests
- Check e-signature request status
- Download signed documents
- Secure API authentication
- Comprehensive logging
- Swagger/OpenAPI documentation
- Clean Architecture implementation

## 🏗️ Solution Structure

The solution follows Clean Architecture principles and is organized into the following projects:

- **Jake.Test.EsigApi.API**: Web API layer containing controllers and configuration
- **Jake.Test.EsigApi.Application**: Application layer with business logic, commands, and queries
- **Jake.Test.EsigApi.Domain**: Domain layer with core business entities and interfaces
- **Jake.Test.EsigApi.Infrastructure**: Infrastructure layer for external services and data access

## 🛠️ Technology Stack

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core
- FluentValidation
- NLog for logging
- Swagger/OpenAPI for documentation
- SQL Server for data storage

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

## 🚀 Getting Started

1. Clone the repository:
```bash
git clone [repository-url]
```

2. Navigate to the solution directory:
```bash
cd Jake.Test.EsigApi
```

3. Restore NuGet packages:
```bash
dotnet restore
```

4. Update the connection strings in `appsettings.json`

5. Run the application:
```bash
dotnet run --project Jake.Test.EsigApi.API
```

## 📝 API Documentation

The API documentation is available through Swagger UI when running the application. Navigate to:
```
https://localhost:[port]/swagger
```

### Key Endpoints

- POST `/api/esignature/send`: Send a new e-signature request
- GET `/api/esignature/status/{id}`: Get the status of an e-signature request
- GET `/api/esignature/download/{id}`: Download a signed document

## 🔍 Logging

The application uses NLog for logging with the following features:
- Structured logging
- Database logging
- Console output
- Configurable log levels

## 🔒 Security

- HTTPS enforcement
- API authentication
- Input validation using FluentValidation
- Secure configuration management

## 🧪 Testing

To run the tests:
```bash
dotnet test
```

## 📦 Dependencies

Key NuGet packages used:
- FluentValidation.AspNetCore (11.3.0)
- NLog (5.3.4)
- NLog.Database (5.3.4)
- NLog.Web.AspNetCore (5.3.15)
- Swashbuckle.AspNetCore (6.5.0)
- System.Data.SqlClient (4.9.0)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📞 Support

For support, please open an issue in the repository or contact the development team. 

## 📝 Notes
POST /esignature
POST /esignature/{id}/send
GET /esignature/{id}/status
GET /esignature/{id}/download
POST /esignature/{id}/cancel
GET /esignature
GET /esignature/{id}
POST /esignature/{id}/resend
