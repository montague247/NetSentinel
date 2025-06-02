using System.Globalization;

namespace NetSentinel.RrdTool;

public abstract class ArgumentsBuilder
{
    protected static void Add(List<string> arguments, string name, string? value)
    {
        if (value != null)
        {
            arguments.Add(name);
            arguments.Add(value);
        }
    }

    protected static void Add(List<string> arguments, string name, bool value)
    {
        if (value)
            arguments.Add(name);
    }

    protected static void Add(List<string> arguments, string name, int? value)
    {
        if (value.HasValue)
        {
            arguments.Add(name);
            arguments.Add(value.Value.ToString(CultureInfo.InvariantCulture));
        }
    }

    protected static void Add(List<string> arguments, List<string>? values)
    {
        if (values == null)
            return;

        foreach (var value in values)
        {
            arguments.Add(value);
        }
    }
}
