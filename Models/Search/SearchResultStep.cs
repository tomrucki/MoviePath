namespace MoviePath.Models.Search;

public class SearchResultStep 
{
    public StepType StepType { get; set; }

    public string Name { get; set; }

    public SearchResultStep(StepType stepType, string name)
    {
        StepType = stepType;
        Name = name;
    }
}
