using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Charon;

namespace NetSentinel.Configuration;

public sealed class ConfigurationEntry
{
    public bool? Enabled { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string? Name { get; internal set; }

    public string? Type { get; set; }

    public Scheduling? Scheduling { get; set; }

    [JsonPropertyName("configuration")]
    public JsonElement? ConfigurationRaw { get; set; }

    public T GetConfiguration<T>()
    {
        if (ConfigurationRaw == null || ConfigurationRaw.Value.ValueKind == JsonValueKind.Null)
            return default!;

        return ConfigurationRaw.Value.Deserialize<T>() ?? throw new InvalidOperationException("Failed to deserialize configuration.");
    }

    public ConfigurationEntry SetConfiguration<T>(T configuration)
    {
        ConfigurationRaw = JsonSerializer.SerializeToElement(configuration);
        Type = typeof(T).GetCustomAttribute<ConfigurationAttribute>()?.Name ?? typeof(T).TypeName();

        return this;
    }
}
