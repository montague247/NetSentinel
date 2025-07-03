namespace NetSentinel.Configuration;

public interface IConfigurationEntryHandler
{
    void Execute(ConfigurationEntry entry, CancellationToken cancellationToken);
}
