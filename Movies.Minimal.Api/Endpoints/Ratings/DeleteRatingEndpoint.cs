using Movies.Minimal.Api.Auth.Extensions;
using Movies.Application.Services.Abstractions;

namespace Movies.Minimal.Api.Endpoints.Ratings;

public static class DeleteRatingEndpoint
{
    public const string Name = "DeleteRating";

    public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoint.Movies.DeleteRating, async (
                Guid id, 
                HttpContext context, 
                IRatingService ratingService,
                CancellationToken cancellationToken) =>
                {
                    var userId = context.GetUserId();
                    var result = await ratingService.DeleteRatingAsync(id, userId!.Value, cancellationToken);
                    return result ? Results.Ok() : Results.NotFound();
                })
            .WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

        return app;
    }
}