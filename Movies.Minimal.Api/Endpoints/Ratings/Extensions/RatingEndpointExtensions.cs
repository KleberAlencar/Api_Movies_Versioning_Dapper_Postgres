namespace Movies.Minimal.Api.Endpoints.Ratings.Extensions;

public static class RatingEndpointExtensions
{
    public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDeleteRating();
        app.MapGetUserRatings();
        app.MapRateMovie();  
        return app;
    }
}