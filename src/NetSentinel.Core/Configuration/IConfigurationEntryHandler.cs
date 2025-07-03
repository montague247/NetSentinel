namespace NetSentinel.Configuration;

public interface IConfigurationEntryHandler
{
    Task Execute(ConfigurationEntry entry, CancellationToken cancellationToken);
}
