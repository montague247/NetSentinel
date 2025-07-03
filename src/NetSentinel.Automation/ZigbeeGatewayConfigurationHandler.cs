using NetSentinel.Configuration;

namespace NetSentinel.Automation
{
    public sealed class ZigbeeGatewayConfigurationHandler : ConfigurationHandlerBase<ZigbeeGatewayConfiguration>
    {
        public override void Execute(ZigbeeGatewayConfiguration configuration, CancellationToken cancellationToken)
        {
            ZigbeeGateway.EnsureServices(configuration, cancellationToken);
        }
    }
}
