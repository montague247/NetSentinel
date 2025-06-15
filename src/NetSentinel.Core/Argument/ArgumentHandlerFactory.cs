using System.Reflection;
using Charon;

namespace NetSentinel.Argument;

public sealed class ArgumentHandlerFactory
{
    private readonly Dictionary<string, Type> _handlers = [];

    public ArgumentHandlerFactory()
    {
        var types = typeof(IArgumentHandler).FindDerivedTypes();

        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<ArgumentHandlerAttribute>();

            if (attribute == null)
                continue;

            _handlers.Add(attribute.Name ?? string.Empty, type);
        }
    }

    public IReadOnlyDictionary<string, Type> Handlers { get { return _handlers; } }

    public IArgumentHandler? GetHandler(string argument)
    {
        if (_handlers.TryGetValue(argument, out var type))
            return (IArgumentHandler?)Activator.CreateInstance(type);

        return default;
    }
}
