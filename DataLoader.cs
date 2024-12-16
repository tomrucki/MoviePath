using MoviePath.Models;
using System.Text.Json;

namespace MoviePath;

public static class DataLoader 
{
    public static async Task<List<MovieDto>> LoadMovies(string pathToJsonFile)
    {
        if (string.IsNullOrEmpty(pathToJsonFile) || File.Exists(pathToJsonFile) == false)
        {
            throw new ArgumentException($"'{pathToJsonFile}' is not a valid file path");
        }

        using var jsonStream = File.OpenRead(pathToJsonFile);
        var moviesData = await JsonSerializer.DeserializeAsync<MovieDataDto>(
            jsonStream, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return moviesData?.Movies ?? new();
    }
}