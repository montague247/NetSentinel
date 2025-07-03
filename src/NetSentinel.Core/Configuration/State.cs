using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetSentinel.Configuration;

public sealed class State
{
    public DateTime? LastExecutionUtc { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Repeated { get; set; }

    [JsonPropertyName("state")]
    public JsonElement? StateRaw { get; set; }

    public T GetState<T>()
    {
        if (StateRaw == null || StateRaw.Value.ValueKind == JsonValueKind.Null)
            return default!;

        return StateRaw.Value.Deserialize<T>() ?? throw new InvalidOperationException("Failed to deserialize state.");
    }

    public State SetState<T>(T state)
    {
        StateRaw = JsonSerializer.SerializeToElement(state);

        return this;
    }
}
