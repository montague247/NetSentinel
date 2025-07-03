using NetSentinel.Configuration;

namespace NetSentinel.Scheduling;

public sealed class Scheduler
{
    private readonly Dictionary<string, LocalState> _localStates = [];

    public async Task Process(Configurations? configurations, States? states, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(configurations);
        ArgumentNullException.ThrowIfNull(states);

        foreach (var configuration in configurations.Entries.Values.Where(s => s.Enabled ?? true && s.Scheduling != null))
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var state = states.GetEntry(configuration.Name!) ?? new();
            var localState = GetLocalState(configuration.Name!, state);

            await configurations.Execute(configuration, cancellationToken);

            if (states.SetEntry(configuration.Name!, state!))
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
