using System.Reflection;
using Charon;
using Charon.IO;
using Serilog;

namespace NetSentinel.Configuration;

public sealed class Configurations
{
    private string? _path;
    private static readonly Dictionary<Type, Type> _handlers = [];

    public Dictionary<string, ConfigurationEntry> Entries { get; set; } = [];

    public Dictionary<string, string> Types { get; set; } = [];

    public static Configurations Load(string? path, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentException("Configuration path cannot be null or empty.", nameof(path));

        Log.Information("Loading configuration from '{Path}'...", path);

        var configurations = JsonExtensions.FromFile<Configurations>(path, false) ?? new();

        configurations._path = path;

        if (FillTypes(configurations.Types))
            configurations.Save(cancellationToken);

        return configurations;
    }

    public States LoadStates(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_path))
            throw new InvalidOperationException("Configuration path is not set.");

        var idx = _path.LastIndexOf('.');
        var path = _path.Insert(idx, "-state");

        return States.Load(path, cancellationToken);
    }

    public ConfigurationEntry[] GetEntries<T>()
    {
        if (Entries == null)
            return [];

        var shortTypeName = typeof(T).GetCustomAttribute<ConfigurationAttribute>()?.Name;
        var typeName = typeof(T).TypeName();

        return Entries
            .Where(x => string.Compare(x.Value.Type, shortTypeName, StringComparison.OrdinalIgnoreCase) == 0 ||
                        string.Compare(x.Value.Type, typeName, StringComparison.OrdinalIgnoreCase) == 0)
            .Select(x => { x.Value.Name = x.Key; return x.Value; })
            .ToArray();
    }

    public async Task<bool> Execute(ConfigurationEntry entry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entry);

        if (!Types.TryGetValue(entry.Type ?? string.Empty, out var typeName))
            typeName = entry.Type;

        if (typeName == null)
        {
            Log.Warning("Configuration type '{Type}' not found for entry '{Name}'.", entry.Type, entry.Name);
            return false;
        }

        var configurationType = Type.GetType(typeName, false);

        if (configurationType == null)
        {
            Log.Warning("Configuration type '{Type}' not found for entry '{Name}'.", typeName, entry.Name);
            return false;
        }

        if (!_handlers.TryGetValue(configurationType, out var handlerType))
        {
            Log.Warning("No handler found for configuration type '{Type}' for entry '{Name}'.", typeName, entry.Name);
            return false;
        }

        if (Activator.CreateInstance(handlerType) is not IConfigurationEntryHandler handler)
        {
            Log.Error("Failed to create handler for configuration type '{Type}' for entry '{Name}'.", typeName, entry.Name);
            return false;
        }

        try
        {
            await handler.Execute(entry, cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error executing configuration entry '{Name}' of type '{Type}'.", entry.Name, typeName);
            return false;
        }

        return true;
    }

    public async Task<bool> Execute(string name, CancellationToken cancellationToken)
    {
        var entry = GetEntry(name);

        if (entry == null)
        {
            Log.Warning("Configuration entry '{Name}' not found.", name);
            return false;
        }

        return await Execute(entry, cancellationToken);
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

    public static bool FillTypes(Dictionary<string, string> types)
    {
        var genericConfigurationHandlerType = typeof(IConfigurationHandler<>);
        var changed = false;

        foreach (var type in genericConfigurationHandlerType.FindDerivedTypes())
        {
            var interfaceType = type.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericConfigurationHandlerType);
            var genericArgument = interfaceType.GetGenericArguments().Single();
            var attribute = genericArgument.GetCustomAttribute<ConfigurationAttribute>();

            if (types.TryAdd(attribute?.Name ?? genericArgument.FullName!, genericArgument.TypeName()!))
                changed = true;

            _handlers.TryAdd(genericArgument, type);
        }

        return changed;
    }
}
