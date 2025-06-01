using System.Text.Json;
using System.Text.Json.Serialization;
using Charon;

namespace NetSentinel.Configuration;

public sealed class ConfigurationEntry
{
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string? Name { get; internal set; }

    public string? Type { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Ref { get; set; }

    public Scheduling? Scheduling { get; set; }

    [JsonPropertyName("configuration")]
    public JsonElement ConfigurationRaw { get; set; }

    public T GetConfiguration<T>()
    {
        return ConfigurationRaw.Deserialize<T>() ?? throw new InvalidOperationException("Failed to deserialize configuration.");
    }

    public ConfigurationEntry SetConfiguration<T>(T configuration)
    {
        ConfigurationRaw = JsonSerializer.SerializeToElement(configuration);
        Type = typeof(T).TypeName();

        return this;
    }
}
