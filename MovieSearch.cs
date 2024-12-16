using System.Collections.Immutable;
using MoviePath.Models;
using MoviePath.Models.Search;

namespace MoviePath;

public class MovieSearch
{
    Dictionary<string, Actor> _actors = new();

    public MovieSearch(List<MovieDto> movies)
    {
        Initialize(movies);
    }

    private void Initialize(List<MovieDto> moviesData)
    {
        _actors.Clear();

        foreach (var movieItem in moviesData)
        {
            if (movieItem.Cast is null || movieItem.Cast.Length == 0)
            {
                continue;
            }

            var movie = new Movie(movieItem.Title ?? "");

            foreach (var actorName in movieItem.Cast)
            {
                if (_actors.TryGetValue(actorName, out var actor) == false)
                {
                    actor = new (actorName);
                    _actors.Add(actorName, actor);
                }

                movie.Actors.Add(actor);
                actor.Movies.Add(movie);
            }
        }
    }

    public string[] GetAllActors()
    {
        return _actors.Keys.Order().ToArray();
    }

    public List<SearchResultStep> FindPath(string actorFrom, string actorTo)
    {
        if (actorFrom == actorTo)
        {
            throw new MovieSearchException("Actor from and actor to can not be the same");
        }

        Actor actorStart;
        if (_actors.TryGetValue(actorFrom, out actorStart) == false)
        {
            throw new MovieSearchException($"Not a valid actor name '{actorFrom}'");
        }

        Actor actorEnd;
        if (_actors.TryGetValue(actorTo, out actorEnd) == false)
        {
            throw new MovieSearchException($"Not a valid actor name '{actorTo}'");
        }

        HashSet<Movie> testeddMovies = new(actorStart.Movies);
        Queue<ImmutableList<SearchStep>> toTestMovies = new (actorStart.Movies
            .Select(m => ImmutableList.Create(new SearchStep(actorStart, m))));

        while (toTestMovies.Count > 0) 
        {
            var testedMoviePath = toTestMovies.Dequeue();
            var testedMovie = testedMoviePath.Last().Movie;

            foreach (var actor in testedMovie.Actors)
            {
                if (actor == actorEnd)
                {
                    return CreateSearchResult(testedMoviePath, actorEnd);
                }

                foreach (var actorMovie in actor.Movies)
                {
                    if (testeddMovies.Contains(actorMovie) == false)
                    {
                        var nextPath = testedMoviePath.Add(new SearchStep(actor, actorMovie));
                        toTestMovies.Enqueue(nextPath);
                        testeddMovies.Add(actorMovie);
                    }
                }
            }
        }

        return new();
    }   

    private static List<SearchResultStep> CreateSearchResult(ImmutableList<SearchStep> steps, Actor actorEnd)
    {
        var result = new List<SearchResultStep>();

        foreach (var item in steps)
        {
            result.Add(new SearchResultStep(StepType.Actor, item.ActorFrom.Name));
            result.Add(new SearchResultStep(StepType.Movie, item.Movie.Title));
        }

        result.Add(new SearchResultStep(StepType.Actor, actorEnd.Name));

        return result;
    }

    class SearchStep
    {
        public Actor ActorFrom { get; set; }
        public Movie Movie { get; set; }

        public SearchStep(Actor actorFrom, Movie movie)
        {
            ActorFrom = actorFrom;
            Movie = movie;
        }
    }
}
