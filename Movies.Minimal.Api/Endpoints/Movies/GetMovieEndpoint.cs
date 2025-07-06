using Movies.Contracts.Responses;
using Movies.Minimal.Api.Mappings;
using Movies.Minimal.Api.Auth.Extensions;
using Movies.Application.Services.Abstractions;

namespace Movies.Minimal.Api.Endpoints.Movies;

public static class GetMovieEndpoint
{
    public const string Name = "GetMovie";

    public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoint.Movies.Get, async (
            string idOrSlug, 
            IMovieService movieService, 
            HttpContext context, 
            CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();

                var movie = Guid.TryParse(idOrSlug, out var id)
                    ? await movieService.GetByIdAsync(id, userId, cancellationToken)
                    : await movieService.GetBySlugAsync(idOrSlug, userId, cancellationToken);
                if (movie is null)
                {
                    return Results.NotFound();
                }

                var response = movie.MapToResponse();
                return TypedResults.Ok(response);               
                
                
            }).WithName(Name)
            .Produces<MovieResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .CacheOutput("MovieCache");
        
        return app;
    }
}