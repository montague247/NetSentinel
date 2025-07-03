using System.Text.Json.Serialization;
using Charon;
using Charon.IO;
using Serilog;

namespace NetSentinel.Configuration;

public sealed class States
{
    private string? _path;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ProcessId { get; set; }

    public Dictionary<string, State> Entries { get; set; } = [];

    public static States Load(string path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("State path cannot be null or empty.", nameof(path));

        Log.Information("Loading states from '{Path}'...", path);

        var states = JsonExtensions.FromFile<States>(path, false) ?? new();

        states._path = path;
        states.ProcessId = Environment.ProcessId;

        states.Save(cancellationToken);

        return states;
    }

    public State? GetEntry(string name)
    {
        if (Entries == null || !Entries.TryGetValue(name, out var entry))
            return default;

        return entry;
    }

    public bool SetEntry(string name, State entry)
    {
        ArgumentNullException.ThrowIfNull(entry);

        Entries ??= [];

        if (Entries.TryGetValue(name, out State? currentEntry))
        {
            if (string.Compare(entry?.ToJson(), currentEntry?.ToJson(), StringComparison.Ordinal) != 0)
            {
                Entries[name] = entry!;

                return true;
            }
        }
        else
        {
            Entries.Add(name, entry);

            return true;
        }

        return false;
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
