using NetSentinel.Configuration;

namespace NetSentinel.Automation
{
    public sealed class ZigbeeGatewayConfigurationHandler : IConfigurationHandler<ZigbeeGatewayConfiguration>
    {
        public void Execute(ZigbeeGatewayConfiguration configuration)
        {
            ZigbeeGateway.EnsureServices(configuration);
        }
    }
}
