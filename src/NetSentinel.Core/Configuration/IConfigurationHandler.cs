namespace NetSentinel.Configuration;

public interface IConfigurationHandler<T> : IConfigurationEntryHandler
    where T : class
{
    void Execute(T configuration, CancellationToken cancellationToken);
}
