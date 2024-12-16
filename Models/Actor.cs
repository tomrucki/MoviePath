namespace MoviePath.Models;

public class Actor
{
    public string Name { get; set; }
    public List<Movie> Movies { get; set; }

    public Actor(string name)
    {
        Name = name;
        Movies = new();
    }
}