namespace NetSentinel.Configuration;

public interface IConfigurationHandler<T> : IConfigurationEntryHandler
    where T : class
{
    Task Execute(T configuration, CancellationToken cancellationToken);
}
