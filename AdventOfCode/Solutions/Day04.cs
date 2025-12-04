namespace AdventOfCode.Solutions;

public class Day04_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 4);
        var result = input
            .ParseInput()
            .CountAndRemoveAdjecentRollsOfPaper()
            .Count();

        return result.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 4);
        var result = input
            .ParseInput()
            .CountAndRemoveAdjecentRollsOfPaperRecursively()
            .Sum();

        return result.ToString();
    }
}


file static class Day04_2025_Extensions
{
    public static (int row, int col)[] directions =
    [
            (0, 1),  // Right
            (0, -1), // Left
            (1, 0),  // Down
            (-1, 0), // Up
            (1, 1),  // Down-Right
            (1, -1), // Down-Left
            (-1, 1), // Up-Right
            (-1, -1) // Up-Left
    ];

    public static List<List<char>> ParseInput(this IEnumerable<string> lines)
        => [.. lines.Select(x => x.ToList())];

    public static IEnumerable<int> CountAndRemoveAdjecentRollsOfPaperRecursively(this List<List<char>> grid)
    {
        var removed = grid.CountAndRemoveAdjecentRollsOfPaper().ToList();
         return removed switch
        {
            [] => [],
            _ => [.. removed, .. grid.CountAndRemoveAdjecentRollsOfPaperRecursively()] // concatenate lists of ints and concatenate recursive call until empty
        };
    }

    public static IEnumerable<int> CountAndRemoveAdjecentRollsOfPaper(this List<List<char>> grid)
    {
        var paperRollsToRemove = new List<(int row, int col)>();
        for (var row = 0; row < grid.Count; row++)
            for (var col = 0; col < grid[row].Count; col++)
            {
                var position = grid[row][col];
                if (position is '.')
                    continue;

                var totalAdjecentPaperRolls = directions.Count(direction =>
                {
                    var (r , c) = (row + direction.row, col + direction.col);
                    return r >= 0 && r < grid.Count &&
                           c >= 0 && c < grid[r].Count &&
                           grid[r][c] == position;
                });

                if (totalAdjecentPaperRolls < 4)
                    paperRollsToRemove.Add((row, col));
            }

        foreach (var (row, col) in paperRollsToRemove)
        {
            grid[row][col] = '.';
            yield return 1;
        }
    }
}