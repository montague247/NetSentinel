namespace NetSentinel.Configuration;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ConfigurationAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
