namespace AdventOfCode.Solutions;

public class Day01_2025(AdventOfCodeClient client) 
    : ISolution
{
    public async Task<string> SolvePart1()
    {
        var input = await client.GetInputAsync(2025, 1);
        var zeroPositions = input
            .Select(x => x.First() is 'L' ? -int.Parse(x[1..]) : int.Parse(x[1..]))
            .DialRotations(startDialLocation: 50)
            .Where(x => x.dialPosition == 0)
            .Count();

        return zeroPositions.ToString();
    }

    public async Task<string> SolvePart2()
    {
        var input = await client.GetInputAsync(2025, 1);
        var zeroClicks = input
            .Select(x => x.First() is 'L' ? -int.Parse(x[1..]) : int.Parse(x[1..]))
            .DialRotations(startDialLocation: 50)
            .Select(x => x.zeroClicks)
            .Sum();

        return zeroClicks.ToString();
    }
}


static class Day01_2025_Extensions
{
    public static IEnumerable<(int dialPosition, int zeroClicks)> DialRotations(this IEnumerable<int> rotations, int startDialLocation = 50)
    {
        var currentDialLocation = startDialLocation;
        foreach (var rotation in rotations)
        {
            (currentDialLocation, var zeroClicks) = currentDialLocation.TurnDialWithZeroClicks(rotation);
            yield return (currentDialLocation, zeroClicks);
        }
    }

    public static (int dialPosition, int zeroClicks) TurnDialWithZeroClicks(this int dialLocation, int rotation)
    {
        var dialPosition = TurnDial(dialLocation, rotation);
        var zeroClicks = Enumerable.Range(0, rotation.Absolute())
            .Count(x => dialLocation.TurnDial(rotation.Sign() * x) == 0);

        return (dialPosition, zeroClicks);
    }

    public static int TurnDial(this int dialLocation, int rotation)
        => ((dialLocation + rotation) % 100 + 100) % 100;


    public static int Absolute(this int value)
        => value < 0 ? -value : value;

    public static int Sign(this int value)
        => value < 0 ? -1 : 1;
}