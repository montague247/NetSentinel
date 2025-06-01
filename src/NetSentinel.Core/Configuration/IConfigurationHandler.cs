namespace NetSentinel.Configuration;

public interface IConfigurationHandler<T>
    where T : class
{
    void Execute(T configuration);
}
