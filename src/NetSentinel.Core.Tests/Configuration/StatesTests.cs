using NetSentinel.Configuration;

namespace NetSentinel.Core.Tests.Configuration;

public sealed class StatesTests
{
    [Fact]
    public void LoadNotExisting()
    {
        var path = Path.Combine($"{nameof(StatesTests)}-{nameof(LoadNotExisting)}.json");
        var states = Configurations.Load(path, default).LoadStates(default);

        Assert.NotNull(states);
        Assert.NotNull(states.Entries);
    }

    [Fact]
    public void LoadExisting()
    {
        var path = Path.Combine($"{nameof(StatesTests)}-{nameof(LoadExisting)}.json");

        var states = Configurations.Load(path, default).LoadStates(default);
        states.SetEntry("a", new());
        states.SetEntry("b", new());
        states.SetEntry("c", new());
        states.Save(default);

        states = Configurations.Load(path, default).LoadStates(default);

        Assert.NotNull(states);
        Assert.NotNull(states.Entries);
        Assert.Equal(3, states.Entries.Count);
    }

    [Fact]
    public void GetEntry()
    {
        var states = new States();
        states.SetEntry("a", new());
        states.SetEntry("b", new());
        states.SetEntry("c", new());

        var state = states.GetEntry("a");
        Assert.NotNull(state);
        Assert.Null(state.GetState<object>());

        state = states.GetEntry("b");
        Assert.NotNull(state);
        state.SetState(new LocalState());
        Assert.NotNull(state.GetState<LocalState>());
    }
}
