using System.Text.Json.Serialization;

namespace NetSentinel.Configuration;

public sealed class Scheduling
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Ref { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Interval? Interval { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Repeat? Repeat { get; set; }
}
