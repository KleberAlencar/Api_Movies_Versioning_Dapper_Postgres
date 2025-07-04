namespace Movies.Minimal.Api.Endpoints.Extensions;

public static class RatingEndpointExtensions
{
    public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app)
    {
        /*
        app.MapRateMovie();
        app.MapDeleteRating();
        app.MapGetUserRatings();
        */
        return app;
    }
}