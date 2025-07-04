namespace Movies.Application.Database.Scripts;

public static class MovieScript
{
    public const string Create = """INSERT INTO Movies (id, slug, title, yearofrelease) """ +
                                 """VALUES (@Id, @Slug, @Title, @YearOfRelease)""";

    public const string GenreCreate = """INSERT INTO Genres (movieId, name) """ +
                                      """VALUES (@MovieId, @Name)""";

    public const string GetById = """
                                  select m.*, round(avg(r.rating), 1) as rating, myr.rating as userrating
                                  from movies m 
                                  left join ratings r on m.id = r.movieid
                                  left join ratings myr on m.id = myr.movieid and myr.userid = @userId
                                  where id = @id
                                  group by id, userrating
                                  """;

    public const string GetGenresByMovieId = """select name from genres where movieid = @id""";

    public const string GetBySlug = """
                                    select m.*, round(avg(r.rating), 1) as rating, myr.rating as userrating
                                    from movies m 
                                    left join ratings r on m.id = r.movieid
                                    left join ratings myr on m.id = myr.movieid and myr.userid = @userId
                                    where slug = @slug
                                    group by id, userrating
                                    """;

    public const string GetAll =
        """
        select m.*, 
               string_agg(distinct g.name, ',') as genres , 
               round(avg(r.rating), 1) as rating, 
               myr.rating as userrating
        from movies m 
        left join genres g on m.id = g.movieid
        left join ratings r on m.id = r.movieid
        left join ratings myr on m.id = myr.movieid
            and myr.userid = @userId
        where (@title is null or m.title like ('%' || @title || '%'))
          and (@yearofrelease is null or m.yearofrelease = @yearofrelease)
        group by id, userrating 
        """;

    public const string ExistsById = """select count(1) from movies where id = @id""";

    public const string DeleteGenresByMovieId = """delete from genres where movieid = @id""";

    public const string Delete = """delete from movies where id = @id""";

    public const string Update =
        """update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease where id = @Id""";
    
    public const string GetMoviesCount = """
                                         select count(id) from movies where (@title is null or title like ('%' || @title || '%'))
                                            and (@yearOfRelease is null or yearofrelease = @yearOfRelease)
                                         """;
}