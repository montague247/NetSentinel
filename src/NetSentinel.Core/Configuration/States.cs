using System.Text.Json.Serialization;
using Charon;
using Charon.IO;

namespace NetSentinel.Configuration;

public sealed class States
{
    private string? _path;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ProcessId { get; set; }

    public Dictionary<string, State> Entries { get; set; } = [];

    public static States Load(string path)
    {
        var config = JsonExtensions.FromFile<States>(path, false) ?? new();

        config._path = path;
        config.ProcessId = Environment.ProcessId;

        return config;
    }

    public State? GetEntry(string name)
    {
        if (Entries == null || !Entries.TryGetValue(name, out var entry))
            return default;

        return entry;
    }

    public States SetEntry(string name, State entry)
    {
        Entries ??= [];

        if (!Entries.TryAdd(name, entry))
            Entries[name] = entry;

        return this;
    }

    public void Save(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_path))
            throw new InvalidOperationException("State path is not set.");

        var tempPath = string.Concat(_path, ".temp");

        this.ToJson(tempPath);

        FileComparer.Move(tempPath, _path, cancellationToken);
    }
}
