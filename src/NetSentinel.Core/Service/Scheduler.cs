using NetSentinel.Configuration;

namespace NetSentinel.Service;

public sealed class Scheduler
{
    private readonly Dictionary<string, LocalState> _localStates = [];

    public void Process(Configurations? configurations, States? states, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(configurations);
        ArgumentNullException.ThrowIfNull(states);

        foreach (var configuration in configurations.Entries.Where(s => s.Value.Enabled ?? true && s.Value.Scheduling != null))
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var state = states.GetEntry(configuration.Key) ?? new();
            var localState = GetLocalState(configuration.Key, state);

            if (states.SetEntry(configuration.Key, state!))
            {
                // Save the state if it has changed
                states.Save(cancellationToken);
            }
        }
    }

    private LocalState GetLocalState(string key, State state)
    {
        if (_localStates.TryGetValue(key, out var localState))
            return localState;

        localState = new()
        {
            Repeated = state.Repeated
        };

        _localStates.Add(key, localState);

        return localState;
    }
}
