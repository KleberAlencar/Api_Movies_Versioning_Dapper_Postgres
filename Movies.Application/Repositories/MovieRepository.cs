using Dapper;
using Movies.Application.Models;
using Movies.Application.Database.Scripts;
using Movies.Application.Database.Abstractions;
using Movies.Application.Repositories.Abstractions;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        var result =
            await connection.ExecuteAsync(new CommandDefinition(MovieScript.Create, movie,
                cancellationToken: cancellationToken));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition(MovieScript.GenreCreate,
                    new { MovieId = movie.Id, Name = genre }, cancellationToken: cancellationToken));
            }
        }

        transaction.Commit();

        return result > 0;
    }

    public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(MovieScript.GetById,
            new { id, userId }, cancellationToken: cancellationToken));

        if (movie is null)
        {
            return null;
        }

        var genres =
            await connection.QueryAsync<string>(new CommandDefinition(MovieScript.GetGenresByMovieId,
                new { id }, cancellationToken: cancellationToken));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(MovieScript.GetBySlug,
            new { slug, userId }, cancellationToken: cancellationToken));

        if (movie is null)
        {
            return null;
        }

        var genres =
            await connection.QueryAsync<string>(new CommandDefinition(MovieScript.GetGenresByMovieId,
                new { id = movie.Id }, cancellationToken: cancellationToken));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        var orderClause = string.Empty;
        if (options.SortField is not null)
        {
            orderClause = $"""
                           , m.{options.SortField}
                           order by m.{options.SortField} {(options.SortOrder == SortOrder.Ascending ? "asc" : "desc" )}
                           """;
        }

        var result =
            await connection.QueryAsync(new CommandDefinition(
                MovieScript.GetAll + orderClause + " " + CommonScript.Pagination,
                new
                {
                    userId = options.UserId,
                    title = options.Title,
                    yearofrelease = options.YearOfRelease,
                    pageSize = options.PageSize,
                    pageOffset = (options.Page - 1) * options.PageSize
                },
                cancellationToken: cancellationToken));

        return result.Select(m => new Movie
        {
            Id = m.id,
            Title = m.title,
            YearOfRelease = m.yearofrelease,
            Rating = (float?)m.rating,
            UserRating = (int?)m.userrating,
            Genres = Enumerable.ToList(m.genres.Split(','))
        });
    }

    public async Task<bool> UpdateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition(MovieScript.DeleteGenresByMovieId,
            new { id = movie.Id }, cancellationToken: cancellationToken));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition(MovieScript.GenreCreate,
                new { MovieId = movie.Id, Name = genre }, cancellationToken: cancellationToken));
        }

        var result =
            await connection.ExecuteAsync(new CommandDefinition(MovieScript.Update, movie,
                cancellationToken: cancellationToken));
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition(MovieScript.DeleteGenresByMovieId, new { id },
            cancellationToken: cancellationToken));

        var result =
            await connection.ExecuteAsync(new CommandDefinition(MovieScript.Delete, new { id },
                cancellationToken: cancellationToken));
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(MovieScript.ExistsById,
            new { id }, cancellationToken: cancellationToken));
    }

    public async Task<int> GetCountAsync(string? title, int? yearOfRelease,
        CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleAsync<int>(new CommandDefinition(MovieScript.GetMoviesCount,
            new { title, yearOfRelease }, cancellationToken: cancellationToken));
    }
}