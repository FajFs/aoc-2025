namespace AdventOfCode.Solutions;

public class Day03_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 3);
        var result = input
            .ParseInput()
            .CalculateLargestNumberCombination(2)
            .Sum();

        return result.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 3);
        var result = input
            .ParseInput()
            .CalculateLargestNumberCombination(12)
            .Sum();

        return result.ToString();
    }
}


file static class Day03_2025_Extensions
{
    public static IEnumerable<List<double>> ParseInput(this IEnumerable<string> lines)
        => lines.Select(x => x.Select(y => double.Parse(y.ToString())).ToList());

    public static IEnumerable<double> CalculateLargestNumberCombination(this IEnumerable<List<double>> lists, int combinationLength)
        => lists.Select(x => x.CalculateLargestNumberCombination(combinationLength));

    public static double CalculateLargestNumberCombination(this List<double> numbers, int combinationLength)
    {
        var resultString = new StringBuilder();
        for (var i = 1; i <= combinationLength; i++)
        {
            // Find the largest number within the range that allows enough numbers to remain for the rest of the combination
            var largestNumber = numbers[..^(combinationLength - i)].Max();
            var largestNumberIndex = numbers.IndexOf(largestNumber);

            // Remove all numbers up to and including the largest number
            numbers = numbers[(largestNumberIndex + 1)..];
            resultString.Append(largestNumber);
        }
        return double.Parse(resultString.ToString());
    }
}