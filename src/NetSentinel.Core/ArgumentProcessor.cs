using System.Reflection;
using NetSentinel.ArgumentHandling;
using Serilog;

namespace NetSentinel;

public static class ArgumentProcessor
{
    public static IArgumentHandler[] Process(out IGlobalOptions options, params string[] arguments)
    {
        var factory = new ArgumentHandlerFactory();
        var handlers = new List<IArgumentHandler>();
        var globalHandler = new GlobalArgumentHandler();

        for (int index = 0; index < arguments.Length; index++)
        {
            if (index == 0)
                globalHandler.Process(arguments, ref index);

            var argument = arguments[index++];
            var handler = factory.GetHandler(argument);

            if (handler == null)
            {
                Log.Warning("Unknown argument: {Argument}", argument);

                continue;
            }

            handlers.Add(handler);

            handler.Process(arguments, ref index);
            index--;
        }

        options = globalHandler;

        return [.. handlers];
    }

    public static void Execute(IGlobalOptions options, params IArgumentHandler[] handlers)
    {
        foreach (var handler in handlers)
        {
            handler.Execute(options);
        }
    }

    public static void GenerateHelp()
    {
        var factory = new ArgumentHandlerFactory();

        Console.WriteLine("Available arguments:");

        var indent = 1;

        foreach (var handler in factory.Handlers.OrderBy(s => s.Value.GetCustomAttribute<ArgumentHandlerAttribute>()?.Name))
        {
            var attribute = handler.Value.GetCustomAttribute<ArgumentHandlerAttribute>();

            if (attribute == null)
                continue;

            if (attribute.Name == null)
                Console.WriteLine(attribute.Description);
            else
                Console.WriteLine($"  {attribute.Name} - {attribute.Description}");

            GenerateHelp(handler.Value, indent);

            if (attribute.Name != null)
                continue;

            indent = 2;
            Console.WriteLine("Specific options:");
        }
    }

    private static void GenerateHelp(Type type, int indent)
    {
        var attributes = GetAttributes(type).ToArray();

        if (attributes.Length == 0)
            return;

        var indentString = new string(' ', indent * 2);
        var totalWidth = attributes.Max(v => v.Key.Name.Length + GetLength(v.Value));

        foreach (var propertyAttribute in GetAttributes(type))
        {
            var optionName = propertyAttribute.Key.Name;

            if (propertyAttribute.Value.Count > 0)
                optionName = string.Concat(optionName, ' ', string.Join(' ', propertyAttribute.Value.Select(s => $"{{{s.Name}}}").Distinct()));

            Console.WriteLine($"{indentString}{optionName.PadRight(totalWidth)} - {propertyAttribute.Key.Description}");

            if (propertyAttribute.Value.Count == 0)
                continue;

            var valueTotalWidth = propertyAttribute.Value.Max(v => v.Name.Length);

            foreach (var valueAttribute in propertyAttribute.Value)
            {
                Console.WriteLine($"{indentString}  {valueAttribute.Name.PadRight(valueTotalWidth)} - {valueAttribute.Value}: {valueAttribute.Description}");
            }
        }
    }

    private static int GetLength(List<ArgumentHandlerPropertyValueAttribute> values)
    {
        if (values.Count == 0)
            return 0;

        return values.Max(s => s.Name.Length + 2);
    }

    private static IEnumerable<KeyValuePair<ArgumentHandlerPropertyAttribute, List<ArgumentHandlerPropertyValueAttribute>>> GetAttributes(Type type)
    {
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            KeyValuePair<ArgumentHandlerPropertyAttribute, List<ArgumentHandlerPropertyValueAttribute>>? kvAttribute = null;

            foreach (var attribute in property.GetCustomAttributes())
            {
                if (attribute is ArgumentHandlerPropertyAttribute propertyAttribute)
                {
                    if (kvAttribute.HasValue)
                        yield return kvAttribute.Value;

                    kvAttribute = new KeyValuePair<ArgumentHandlerPropertyAttribute, List<ArgumentHandlerPropertyValueAttribute>>(propertyAttribute, []);

                    continue;
                }
                else if (attribute is ArgumentHandlerPropertyValueAttribute valueAttribute)
                {
                    if (!kvAttribute.HasValue)
                        throw new InvalidOperationException("ArgumentHandlerPropertyValueAttribute found without preceding ArgumentHandlerPropertyAttribute");

                    kvAttribute.Value.Value.Add(valueAttribute);
                }
            }

            if (kvAttribute.HasValue)
                yield return kvAttribute.Value;
        }
    }
}
