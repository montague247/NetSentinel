namespace NetSentinel.Configuration;

public abstract class ConfigurationHandlerBase<T> : IConfigurationHandler<T>
    where T : class
{
    public void Execute(ConfigurationEntry entry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entry);

        var configuration = entry.GetConfiguration<T>();

        Execute(configuration, cancellationToken);
    }

    public abstract void Execute(T configuration, CancellationToken cancellationToken);
}
