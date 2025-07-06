using Movies.Minimal.Api.Endpoints.Movies.Extensions;
using Movies.Minimal.Api.Endpoints.Ratings.Extensions;

namespace Movies.Minimal.Api.Endpoints.Extensions;

public static class EndpointExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapMovieEndpoints();
        app.MapRatingEndpoints();
        return app;
    }
}