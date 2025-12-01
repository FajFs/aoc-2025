namespace AdventOfCode.Clients;

public class AdventOfCodeClient(
    HttpClient http, 
    ILogger<AdventOfCodeClient> logger)
{
    public async Task<IEnumerable<string>> GetInputAsync(int year, int day)
    {
        var path = Path.Combine("Inputs", $"{year}_day{day:D2}.txt");
        if (File.Exists(path)) 
            return await File.ReadAllLinesAsync(path);

        logger.LogInformation("Fetching {Year} Day {Day}", year, day);
        var input = await http.GetStringAsync($"{year}/day/{day}/input");
        
        Directory.CreateDirectory("Inputs");
        await File.WriteAllTextAsync(path, input);

        return input.Split(['\n'], StringSplitOptions.RemoveEmptyEntries);
    }
}
