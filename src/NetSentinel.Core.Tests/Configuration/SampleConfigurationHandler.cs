using NetSentinel.Configuration;

namespace NetSentinel.Core.Tests.Configuration;

public sealed class SampleConfigurationHandler : ConfigurationHandlerBase<SampleConfiguration>
{
    public override Task Execute(SampleConfiguration configuration, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
