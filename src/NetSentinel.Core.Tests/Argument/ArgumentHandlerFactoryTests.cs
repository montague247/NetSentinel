using NetSentinel.Argument;

namespace NetSentinel.Core.Tests.Argument;

public sealed class ArgumentHandlerFactoryTests
{
 [Fact]
    public void GetHandler()
    {
        var factory = new ArgumentHandlerFactory();
        Assert.NotNull(factory.GetHandler(string.Empty));
        Assert.NotNull(factory.GetHandler("--speed-test"));
    }
}
