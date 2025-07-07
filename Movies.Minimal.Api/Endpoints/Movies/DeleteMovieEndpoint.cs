using Movies.Minimal.Api.Constants;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Application.Services.Abstractions;

namespace Movies.Minimal.Api.Endpoints.Movies;

public static class DeleteMovieEndpoint
{
    public const string Name = "DeleteMovie";

    public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoint.Movies.Delete, async (
            Guid id,
            IMovieService movieService,
            IOutputCacheStore outputCacheStore,
            CancellationToken cancellationToken) =>
        {
            var deleted = await movieService.DeleteByIdAsync(id, cancellationToken);
            if (!deleted)
            {
                return Results.NotFound();
            }

            await outputCacheStore.EvictByTagAsync("movies", cancellationToken);
            return Results.Ok();
        })
        .WithName(Name)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)        
        .RequireAuthorization(AuthConstants.AdminUserPolicyName);;
        
        return app;
    }
}