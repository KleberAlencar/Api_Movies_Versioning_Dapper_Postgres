using Movies.Contracts.Responses;
using Movies.Minimal.Api.Auth.Extensions;
using Movies.Application.Services.Abstractions;

namespace Movies.Minimal.Api.Endpoints.Ratings;

public static class GetUserRatingsEndpoint
{
    public const string Name = "GetUserRatings";

    public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoint.Ratings.GetUserRatings, async (
            HttpContext context,
            IRatingService ratingService,
            CancellationToken cancellationToken) =>
        {
            var userId = context.GetUserId();
            var ratings = await ratingService.GetRatingsForUserAsync(userId!.Value, cancellationToken);
            return TypedResults.Ok(ratings);
        }).WithName(Name)
        .Produces<MovieRatingResponse>(StatusCodes.Status200OK)
        .RequireAuthorization();

        return app;
    }
}