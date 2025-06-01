namespace NetSentinel.Configuration;

public sealed class Interval
{
    public IntervalType Type { get; set; } = IntervalType.Seconds;

    public int Value { get; set; } = 60;
}
