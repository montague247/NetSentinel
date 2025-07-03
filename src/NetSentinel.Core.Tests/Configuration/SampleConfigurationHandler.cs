using NetSentinel.Configuration;

namespace NetSentinel.Core.Tests.Configuration;

public sealed class SampleConfigurationHandler : ConfigurationHandlerBase<SampleConfiguration>
{
    public override void Execute(SampleConfiguration configuration, CancellationToken cancellationToken)
    {
        // nothing to do here
    }
}
