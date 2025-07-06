namespace Movies.Minimal.Api.Endpoints.Movies.Extensions;

public static class MovieEndpointExtensions
{
    public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateMovie();        
        app.MapDeleteMovie();
        app.MapGetAllMovies();
        app.MapGetMovie();
        app.MapUpdateMovie();
        return app;
    }
}