using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Movies.Minimal.Api.Mappings;
using Movies.Minimal.Api.Auth.Extensions;
using Movies.Application.Services.Abstractions;

namespace Movies.Minimal.Api.Endpoints.Movies;

public static class GetAllMoviesEndpoint
{
    public const string Name = "GetMovies";

    public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoint.Movies.GetAll, async (
                [AsParameters] GetAllMoviesRequest request,
                IMovieService movieService,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var userId = context.GetUserId();
                var options = request.MapToOptions().WithUserId(userId);
                var movies = await movieService.GetAllAsync(options, cancellationToken);
                var moviesCount =
                    await movieService.GetCountAsync(options.Title, options.YearOfRelease, cancellationToken);

                var response = movies.MapToResponse(
                    request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
                    request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                    moviesCount);
                return TypedResults.Ok(response);
            }).WithName(Name)
            .Produces<MoviesResponse>(StatusCodes.Status200OK)
            .CacheOutput("MovieCache");            
        
        return app;
    }
}