using NetSentinel.Configuration;

namespace NetSentinel.Automation
{
    public sealed class ZigbeeGatewayConfigurationHandler : ConfigurationHandlerBase<ZigbeeGatewayConfiguration>
    {
        public override Task Execute(ZigbeeGatewayConfiguration configuration, CancellationToken cancellationToken)
        {
            ZigbeeGateway.EnsureServices(configuration, cancellationToken);

            return Task.CompletedTask;
        }
    }
}
