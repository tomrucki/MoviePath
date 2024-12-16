namespace MoviePath.Models;

public class MovieDto 
{
    public string? Title { get; set; }
    public string[]? Cast { get; set; }
}

public class MovieDataDto 
{
    public List<MovieDto>? Movies { get; set; }
}