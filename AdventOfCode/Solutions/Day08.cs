namespace AdventOfCode.Solutions;

public class Day08_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 8);
        var junctionBoxes = input.ToJunctionBoxes();

        //materialize connections up to 1000 connections
        junctionBoxes.ConnectAllJunctionBoxes(1000).ToList();
        var result = junctionBoxes.ThreeLargestCircuitsProduct();

        return result.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 8);
        var result = input
            .ToJunctionBoxes()
            .ConnectAllJunctionBoxes()
            .Select(x => x.From.X * x.To.X)
            .First();

        return result.ToString();
    }
}

record JunctionBox(double X, double Y, double Z, List<JunctionBox> ConnectedBoxes);
record Connection(JunctionBox From, JunctionBox To, double Distance);

partial class Regexes
{
    [GeneratedRegex(@"(\d+),(\d+),(\d+)")]
    public static partial Regex JunctionBoxRegex();
}

file static class Day08_2025_Extensions
{
    public static List<JunctionBox> ToJunctionBoxes(this IEnumerable<string> input)
        => [.. input.Select(line => Regexes.JunctionBoxRegex().Match(line))
            .Where(match => match.Success)
            .Select(match => new JunctionBox(
                double.Parse(match.Groups[1].Value),
                double.Parse(match.Groups[2].Value),
                double.Parse(match.Groups[3].Value),
                []))];

    public static double DistanceTo(this JunctionBox a, JunctionBox b)
        => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));

    public static void Connect(this JunctionBox a, JunctionBox b)
    {
        a.ConnectedBoxes.Add(b);
        b.ConnectedBoxes.Add(a);
    }

    public static IEnumerable<Connection> ConnectAllJunctionBoxes(
        this List<JunctionBox> boxes,
        int? boxesToConnect = null)
    {
        var connections = boxes
            .SelectMany(a => boxes
                .Where(b => a != b)
                .Select(b => new Connection(a, b, a.DistanceTo(b))))
            .DistinctBy(c => c.Distance)
            .OrderBy(c => c.Distance)
            .AsEnumerable();

        if (boxesToConnect.HasValue)
            connections = connections.Take(boxesToConnect.Value);

        foreach (var connection in connections)
        {
            connection.From.Connect(connection.To);
            
            // All boxes are connected if the circuit count from one box equals the total number of boxes
            if (connection.From.CircuitCount() == boxes.Count)
                yield return connection;
        }
    }

    public static double CircuitCount(
        this JunctionBox box, 
        HashSet<JunctionBox>? visited = null)
    {
        var totalCount = 1d;
        visited ??= [];
        if (visited.Contains(box))
            return 0;

        visited.Add(box);

        foreach (var connectedBox in box.ConnectedBoxes)
            totalCount += connectedBox.CircuitCount(visited);

        return totalCount;
    }

    public static double ThreeLargestCircuitsProduct(this List<JunctionBox> boxes)
    {
        // Keep track of visited boxes to avoid counting the same circuit multiple times
        var visited = new HashSet<JunctionBox>();
        return boxes
            .Where(box => !visited.Contains(box))
            .Select(box => box.CircuitCount(visited))
            .Where(x => x > 0)
            .OrderByDescending(s => s)
            .Take(3)
            .Aggregate((a, b) => a * b);
    }
}
