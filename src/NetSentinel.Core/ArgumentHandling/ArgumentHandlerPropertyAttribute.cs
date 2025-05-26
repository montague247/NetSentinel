namespace NetSentinel.ArgumentHandling
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public sealed class ArgumentHandlerPropertyAttribute(string name, string description) : Attribute
    {
        public string Name { get; } = name;

        public string Description { get; } = description;
    }
}
