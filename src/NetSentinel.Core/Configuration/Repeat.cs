using System.Text.Json.Serialization;

namespace NetSentinel.Configuration;

public sealed class Repeat
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Static { get; set; }

    public int Count { get; set; } = 1;
}
