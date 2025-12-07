namespace AdventOfCode.Solutions;

public class Day07_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 7);
        var result = input
            .ParseInput()
            .PropagateBeams()
            .BeamsSplitCount;

        return result.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 7);
        var result = input
            .ParseInput()
            .PropagateBeams()
            .TotalActiveBeams;

        return result.ToString();
    }
}

file static class Day07_2025_Extensions
{
    public static List<List<char>> ParseInput(this IEnumerable<string> lines)
        => [.. lines.Select(x => x.Select(y => y).ToList())];

    public static (int BeamsSplitCount, double TotalActiveBeams) PropagateBeams(
        this List<List<char>> points)
    {
        var beamsSplitCount = 0;

        var beams = new double[points[0].Count];
        beams[points[0].IndexOf('S')] = 1;

        for (var row = 1; row < points.Count; row++)
            for (var col = 0; col < points[row].Count; col++)
            {
                var activeBeams = beams[col];
                if (activeBeams == 0 || points[row][col] != '^')
                    continue;

                beams[col - 1] += activeBeams;
                beams[col + 1] += activeBeams;
                beams[col] = 0;

                beamsSplitCount++;
            }

        return (beamsSplitCount, beams.Sum());
    }
}
