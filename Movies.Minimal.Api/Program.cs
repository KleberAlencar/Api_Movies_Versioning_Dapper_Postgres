using Movies.Minimal.Api.Endpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapApiEndpoints();

app.Run();
