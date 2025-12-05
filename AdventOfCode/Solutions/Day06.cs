namespace AdventOfCode.Solutions;

public class Day06_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 6);
        var result = input;

        return result.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 6);
        var result = input;

        return result.ToString();
    }
}


file static class Day06_2025_Extensions
{
    public static List<List<char>> ParseInput(this IEnumerable<string> lines)
        => [.. lines.Select(x => x.ToList())];
}