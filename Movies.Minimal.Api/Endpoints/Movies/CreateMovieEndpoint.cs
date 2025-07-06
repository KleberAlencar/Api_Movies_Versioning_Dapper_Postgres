using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Movies.Minimal.Api.Mappings;
using Movies.Minimal.Api.Constants;
using Microsoft.AspNetCore.OutputCaching;
using Movies.Application.Services.Abstractions;

namespace Movies.Minimal.Api.Endpoints.Movies;

public static class CreateMovieEndpoint
{
    public const string Name = "CreateMovie";

    public static IEndpointRouteBuilder MapCreateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoint.Movies.Create, async (
            CreateMovieRequest request,
            IMovieService movieService,
            IOutputCacheStore outputCacheStore,
            CancellationToken cancellationToken) =>
        {
            var movie = request.MapToMovie();
            await movieService.CreateAsync(movie, cancellationToken);
            await outputCacheStore.EvictByTagAsync("movies", cancellationToken);
            var response = movie.MapToResponse();
            return TypedResults.CreatedAtRoute(response, GetMovieEndpoint.Name, new { isOrSlug = response.Id });
        })
        .WithName(Name)
        .Produces<MovieResponse>(StatusCodes.Status201Created)
        .Produces<ValidationFailureResponse>(StatusCodes.Status400BadRequest)
        .RequireAuthorization(AuthConstants.TrustedMemberPolicyName);
        return app;
    }
}