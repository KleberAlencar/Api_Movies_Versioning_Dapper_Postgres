namespace Movies.Application.Database.Scripts;

public static class RatingScript
{
    public const string GetByMovieId = """
                                       select round(avg(r.rating), 1)
                                       from ratings r 
                                       where movieId = @movieId
                                       """;

    public const string GetByMovieIdAndUserId = """
                                                select round(avg(rating), 1),
                                                (select rating
                                                 from ratings
                                                 where movieId = @movieId
                                                   and userId = @userId
                                                 limit 1)    
                                                from ratings  
                                                where movieid = @movieId
                                                """;

    public const string AddRateMovie = """
                                       insert into ratings (userid, movieid, rating)
                                       values (@userId, @movieId, @rating)
                                       on conflict (userid, movieid) do update 
                                         set rating = @rating
                                       """;

    public const string DeleteRating = """
                                       delete from ratings
                                        where movieid = @movieId
                                          and userid = @userId
                                       """;

    public const string GetRatingsForUser = """
                                            select r.rating, r.movieid, m.slug
                                            from ratings r
                                            inner join movies m on r.movieid = m.id
                                            where userid = @userId
                                            """;
}