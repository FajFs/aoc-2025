namespace AdventOfCode.Solutions;

public static class Extensions
{
    extension<TComparable>(TComparable a) 
        where TComparable 
        : IComparable<TComparable>
    {
        public TComparable Min(TComparable b)
            => a.CompareTo(b) < 0 ? a : b;

        public TComparable Max(TComparable b)
            => a.CompareTo(b) > 0 ? a : b;
    }
}