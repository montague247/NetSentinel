namespace NetSentinel.ArgumentHandling
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ArgumentHandlerAttribute(string? name, string description) : Attribute
    {
        public string? Name { get; } = name;

        public string Description { get; } = description;
    }
}
