namespace NetSentinel.RrdTool
{
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
    }
}
