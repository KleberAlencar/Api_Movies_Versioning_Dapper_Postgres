using Movies.Contracts.Requests;
using Movies.Minimal.Api.Auth.Extensions;
using Movies.Application.Services.Abstractions;

namespace Movies.Minimal.Api.Endpoints.Ratings;

public static class RateMovieEndpoint
{
    public const string Name = "RateMovie";

    public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoint.Ratings.GetUserRatings, async (
            Guid id,
            RateMovieRequest request,
            HttpContext context,
            IRatingService ratingService,
            CancellationToken cancellationToken) =>
        {
            var userId = context.GetUserId();
            var result = await ratingService.RateMovieAsync(id, request.Rating, userId!.Value, cancellationToken);
            return result ? Results.Ok() : Results.NotFound();
        }).WithName(Name)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequireAuthorization();

        return app;
    }
}