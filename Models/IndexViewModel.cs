using MoviePath.Models.Search;

namespace MoviePath.Models;

public class IndexViewModel 
{
    public string[] Actors { get; set; } = [];

    public List<SearchResultStep>? SearchSteps {get; set; }
}