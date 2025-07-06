using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Movies.Minimal.Api.Mappings;
using Movies.Minimal.Api.Constants;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Minimal.Api.Auth.Extensions;
using Movies.Application.Services.Abstractions;

namespace Movies.Minimal.Api.Endpoints.Movies;

public static class UpdateMovieEndpoint
{
    public const string Name = "UpdateMovie";

    public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoint.Movies.Update, async (
            Guid id,
            UpdateMovieRequest request,
            IMovieService movieService,
            IOutputCacheStore outputCacheStore,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var userId = context.GetUserId();
            var movie = request.MapToMovie(id);
            var updatedMovie = await movieService.UpdateAsync(movie, userId, cancellationToken);
            if (updatedMovie is null)
            {
                return Results.NotFound();
            }

            await outputCacheStore.EvictByTagAsync("movies", cancellationToken);
            var response = updatedMovie.MapToResponse();
            return TypedResults.Ok(response);
        })
        .WithName(Name)
        .Produces<MovieResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
        .RequireAuthorization(AuthConstants.TrustedMemberPolicyName);
        
        return app;
    }
}