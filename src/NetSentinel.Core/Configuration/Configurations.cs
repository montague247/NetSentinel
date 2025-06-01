using Charon;
using Charon.IO;

namespace NetSentinel.Configuration;

public sealed class Configurations
{
    private string? _path;

    public Dictionary<string, ConfigurationEntry> Entries { get; set; } = [];

    public static Configurations Load(string path)
    {
        var config = JsonExtensions.FromFile<Configurations>(path, false) ?? new();
        config._path = path;

        return config;
    }

    public ConfigurationEntry[] GetEntries<T>()
    {
        if (Entries == null)
            return [];

        var typeName = typeof(T).TypeName();

        return Entries
            .Where(x => string.Compare(x.Value.Type, typeName, StringComparison.OrdinalIgnoreCase) == 0)
            .Select(x => { x.Value.Name = x.Key; return x.Value; })
            .ToArray();
    }

    public ConfigurationEntry? GetEntry(string name)
    {
        if (Entries == null || !Entries.TryGetValue(name, out var entry))
            return default;

        entry.Name = name;

        return entry;
    }

    public Configurations SetEntry(string name, ConfigurationEntry entry)
    {
        Entries ??= [];

        entry.Name = name;

        if (!Entries.TryAdd(name, entry))
            Entries[name] = entry;

        return this;
    }

    public void Save(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_path))
            throw new InvalidOperationException("Configuration path is not set.");

        var tempPath = string.Concat(_path, ".temp");

        this.ToJson(tempPath);

        FileComparer.Move(tempPath, _path, cancellationToken);
    }
}
