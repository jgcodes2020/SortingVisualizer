namespace SortingVisualizer.Misc;

public static class ParseHelpers
{
    public static TimeSpan ParseCombinedUnits(string str)
    {
        TimeSpan span = TimeSpan.Zero;
        foreach (var part in str.Split(' '))
        {
            if (part.EndsWith("us"))
            {
                var us = double.Parse(part[..^2]);
                span += TimeSpan.FromMicroseconds(us);
            }
            else if (part.EndsWith("ms"))
            {
                var ms = double.Parse(part[..^2]);
                span += TimeSpan.FromMilliseconds(ms);
            }
            else if (part.EndsWith("s"))
            {
                var s = double.Parse(part[..^1]);
                span += TimeSpan.FromSeconds(s);
            }
            else
            {
                throw new FormatException("Each part must end with s, ms, or ns (no support for minutes/hours)");
            }
        }

        return span;
    }
}