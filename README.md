# CRUD API MOVIES

This project is a minimal API for managing movies, built with .NET 8 and C#. It allows users to retrieve movies details by filters, sorting and pagination.

**Technologies Used**
- **C#**: Main programming language.
- **.NET 8**: Framework for building the API.
- **Minimal APIs**: For lightweight, fast API endpoint definitions.
- **API Versioning**: To manage different versions of the API.
- **Health Checks**: For monitoring the health of the API.
- **OpenAPI/Swagger**: For API documentation and testing.
- **Dependency Injection**: For managing service lifetimes and dependencies.
- **Custom Authentication Extensions**: For extracting user information from HTTP context.
- **Cancellation Tokens**: For graceful request cancellation.
- **DBConnection Factory**: For managing database connections.
- **Dapper**: For data access, providing a simple way to map database results to C# objects.
- **PostgreSQL**: Database for storing movie data via docker compose.
- **FluentValidation**: For validating incoming requests.
- **Mapping Layer**: For converting domain models to response DTOs.


### How to execute API

To execute this project, follow these steps:

1. **Clone the repository** to your local machine.
2. **Ensure you have .NET 8 SDK** installed.
3. **Start the PostgreSQL database** using Docker Compose (usually with `docker-compose up -d` in the project root).
4. **Restore dependencies** by running:
   ```
   dotnet restore
   ```
5. **To generate the token execute API in the Movies.Helpers/Identity.Api folder (.NET 7 SDK).
6. **Run the API** with:
   ```
   dotnet run
   ```
7. **Access the API documentation** via Swagger at `http://localhost:<port>/swagger` (replace `<port>` with the configured port).

Make sure your `appsettings.json` or environment variables are configured to connect to the PostgreSQL instance.