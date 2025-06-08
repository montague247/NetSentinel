using NetSentinel.Argument;

namespace NetSentinel.Automation;

[ArgumentHandler("--zigbee-gateway", "Enables Zigbee gateway services (RaspBee II and Zigbee2MQTT)")]
public sealed class ZigbeeGatewayArgumentHandler : IArgumentHandler
{
    public void Execute(IGlobalOptions options)
    {
        ZigbeeGateway.EnsureServices(options);
    }

    public void Process(string[] arguments, ref int index)
    {
        // nothing to do
    }
}
