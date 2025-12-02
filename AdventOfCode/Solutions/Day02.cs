namespace AdventOfCode.Solutions;

public class Day02_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 2);
        var result = input
            .ParseInput()
            .Sum(x => x.GetInvalidInputHalfValidation());

        return result.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 2);
        var result = input
            .ParseInput()
            .Sum(x => x.GetInvalidInputMultiPartValidation());

        return result.ToString();
    }
}


file static class Day02_2025_Extensions
{
    public static IEnumerable<double> ParseInput(this IEnumerable<string> lines)
    {
        var ranges = lines
            .Select(x => x.Split(','))
            .SelectMany(x => x);

        foreach (var range in ranges)
        {
            var parts = range.Split("-")
                .Select(x => double.Parse(string.Concat(x)))
                .ToList();

            for (var i = parts[0]; i <= parts[1]; i++)
                yield return i;
        }
    }
  
    public static double GetInvalidInputHalfValidation(this double number)
    {
        var numberString = number.ToString();
        if (numberString.Length % 2 != 0)
            return 0;

        var parts = numberString
            .Chunk(numberString.Length / 2)
            .Select(x => double.Parse(string.Concat(x)))
            .ToList();

        return parts[0] == parts[1]
            ? number 
            : 0;
    }

    public static double GetInvalidInputMultiPartValidation(this double number)
    {
        var numberString = number.ToString();
        for (var numberStringChunkSize = 1; numberStringChunkSize <= numberString.Length / 2; numberStringChunkSize++)
        {
            var parts = numberString
                .Chunk(numberStringChunkSize)
                .Select(x => double.Parse(string.Concat(x)))
                .ToList();

            if (parts.Count < 2)
                continue;

            if (parts.All(x => x == parts[0]))
                return number;
        }
        return 0;
    }
}