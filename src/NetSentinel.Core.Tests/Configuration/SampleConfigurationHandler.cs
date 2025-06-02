using NetSentinel.Configuration;

namespace NetSentinel.Core.Tests.Configuration;

public sealed class SampleConfigurationHandler : IConfigurationHandler<SampleConfiguration>
{
    public void Execute(SampleConfiguration configuration)
    {
        // nothing to do here
    }
}
