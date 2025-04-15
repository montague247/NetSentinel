namespace NetSentinel.ArgumentHandling
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ArgumentHandlerAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}
