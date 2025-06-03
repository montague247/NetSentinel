using NetSentinel.Configuration;

namespace NetSentinel.Core.Tests.Configuration;

public sealed class StatesTests
{
    [Fact]
    public void LoadNotExisting()
    {
        var path = Path.Combine($"{nameof(StatesTests)}-{nameof(LoadNotExisting)}.json");
        var states = Configurations.Load(path).LoadStates();

        Assert.NotNull(states);
        Assert.NotNull(states.Entries);
    }
}
