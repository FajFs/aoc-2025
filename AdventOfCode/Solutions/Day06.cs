namespace AdventOfCode.Solutions;

public class Day06_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 6);
        var signs = input.ParseSigns();
        var result = input
            .ToStringColumns()
            .ToStringColumnsWithSign(signs)
            .Select(problem => problem.Column
                .ToDoubles()
                .Calculate(problem.Sign))
            .Sum();

        return result.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 6);
        var result = input
            .ToStringColumns()
            .ToStringColumnsWithSign(input.ParseSigns())
            .Select(problem => problem.Column
                .AsRightRead()
                .ToDoubles()
                .Calculate(problem.Sign))
            .Sum();

        return result.ToString();
    }
}

file static class Day06_2025_Extensions
{
    public static List<List<string>> ToStringColumns(this IEnumerable<string> lines)
    {
        var rows = lines.SkipLast(1)
            .Select(x => x.ToList())
            .ToList();
        
        var entries = new List<List<string>>();
        var lastSplitIndex = 0;

        for (var col = 0; col < rows[0].Count; col++)
        {
            var isAllSpaces = rows.All(row => row[col] == ' ');
            var isLastColumn = col == rows[0].Count - 1;

            if (isAllSpaces && !isLastColumn)
            {
                entries.Add([.. rows.Select(row => string.Concat(row.Skip(lastSplitIndex).Take(col - lastSplitIndex)))]);
                lastSplitIndex = col + 1;
            }
            else if (isLastColumn)
                entries.Add([.. rows.Select(row => string.Concat(row.Skip(lastSplitIndex).Take(col - lastSplitIndex + 1)))]);
        }

        return entries;
    }

    public static List<char> ParseSigns(this IEnumerable<string> lines)
        => [.. lines
                .Last()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s[0])];

    public static IEnumerable<(List<string> Column, char Sign)> ToStringColumnsWithSign(
        this List<List<string>> problems,
        List<char> signs)
            => signs
                .Select((sign, index) => (problems[index], sign));

    public static List<double> ToDoubles(this List<string> problemColumn)
        => [.. problemColumn.Select(double.Parse)];

    public static List<string> AsRightRead(this List<string> problemColumn)
    {
        var reversed = new List<string>();
        for (var i = 0; i < problemColumn[0].Length; i++)
            reversed.Add(
                string.Concat(problemColumn.Select(row => row[i])));
        
        return reversed;
    }

    public static double Calculate(this List<double> problem, char sign)
    {
        var firstNumbers = problem[0];
        if (problem.Count == 1)
            return firstNumbers;

        return sign switch
        {
            '+' => firstNumbers + problem[1..].Calculate(sign),
            '*' => firstNumbers * problem[1..].Calculate(sign),
            _ => throw new InvalidOperationException($"Unsupported sign: {sign}")
        };
    }
}