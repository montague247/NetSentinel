namespace NetSentinel.Argument;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
public sealed class ArgumentHandlerPropertyValueAttribute : Attribute
{
    public ArgumentHandlerPropertyValueAttribute(string name, string value, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(description));

        Name = name;
        Value = value;
        Description = description;
    }

    public ArgumentHandlerPropertyValueAttribute(string value, string description)
        : this("value", value, description)
    {
    }

    public string Name { get; }

    public string Value { get; }

    public string Description { get; }
}
