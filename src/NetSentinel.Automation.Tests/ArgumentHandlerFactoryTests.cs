using NetSentinel.Argument;
using Serilog;

namespace NetSentinel.Automation.Tests;

public sealed class ArgumentHandlerFactoryTests
{
    [Fact]
    public void GetHandler()
    {
        Log.Debug("{Name} loaded", typeof(ZigbeeGateway).Assembly.GetName().Name);

        var factory = new ArgumentHandlerFactory();
        Assert.NotNull(factory.GetHandler("--zigbee-gateway"));
    }
}
