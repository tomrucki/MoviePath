namespace MoviePath.Models;

public class Movie 
{
    public string Title { get; set; }
    public List<Actor> Actors { get; set; }

    public Movie(string title)
    {
        Title = title;
        Actors = new();
    }
}
