using System.Collections.Frozen;

namespace AdventOfCode.Solutions;

public class Day05_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 5);
        var ranges = input
            .ToRanges()
            .ToFrozenSet();
        
        var result = input
            .ToIds()
            .Where(id => ranges.Any(range => id.IsInRange(range)))
            .Count(x => x > 0);

        return result.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 5);
        var result = input
            .ToRanges()
            .JoinOverlappingRanges()
            .ToRangeSizes()
            .Sum();

        return result.ToString();
    }
}

file static class Day05_2025_Extensions
{
    public static List<(double X, double Y)> ToRanges(this IEnumerable<string> input)
        => [.. input
            .TakeWhile(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line
                .Select(x => line.Split('-', StringSplitOptions.RemoveEmptyEntries))
                .Select(parts => (double.Parse(parts[0]), double.Parse(parts[1]))))
            .SelectMany(x => x)
            .Distinct()];

    public static List<double> ToIds(this IEnumerable<string> input)
        => [.. input
            .SkipWhile(line => !string.IsNullOrWhiteSpace(line))
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(double.Parse)];

    public static bool IsInRange(this double id, (double X, double Y) range)
        => id >= range.X && id <= range.Y;


    public static List<(double X, double Y)> JoinOverlappingRanges(this List<(double X, double Y)> ranges)
    {
        foreach (var range in ranges) 
        {
            var overlappingRanges = ranges
                .Where(r => r != range)
                .Where(r => range.X.IsInRange(r) || range.Y.IsInRange(r))
                .ToList();

            var newRanges = ranges
                .Except(overlappingRanges)
                .Except([range]);

            if (overlappingRanges.Count > 0)
                newRanges = newRanges.Append((
                    X: range.X.Min(overlappingRanges.Min(r => r.X)),
                    Y: range.Y.Max(overlappingRanges.Max(r => r.Y))
                ));

            if (overlappingRanges.Count > 0)
                return [.. newRanges.ToList().JoinOverlappingRanges()];
        }
        return ranges;
    }

    public static List<double> ToRangeSizes(this IEnumerable<(double X, double Y)> ranges)
        => [.. ranges.Select(range => range.Y - range.X + 1)];
}
