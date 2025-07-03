namespace NetSentinel.Configuration;

public abstract class ConfigurationHandlerBase<T> : IConfigurationHandler<T>
    where T : class
{
    public async Task Execute(ConfigurationEntry entry, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entry);

        var configuration = entry.GetConfiguration<T>();

        await Execute(configuration, cancellationToken);
    }

    public abstract Task Execute(T configuration, CancellationToken cancellationToken);
}
