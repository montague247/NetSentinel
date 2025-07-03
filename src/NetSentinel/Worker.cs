using Charon.Hosting;
using Microsoft.Extensions.Hosting;
using NetSentinel.Configuration;
using NetSentinel.Service;

namespace NetSentinel;

public sealed class Worker(IHostApplicationLifetime hostApplicationLifetime) : WorkerBase(hostApplicationLifetime)
{
    private readonly Scheduler _scheduler = new();
    private Configurations? _configurations;
    private States? _states;

    protected override Task OnStart(CancellationToken cancellationToken)
    {
        _configurations = Configurations.Load(Charon.Hosting.Service.Options?.ConfigPath, cancellationToken);
        _states = _configurations.LoadStates(cancellationToken);

        return base.OnStart(cancellationToken);
    }

    protected override Task OnExecute(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        _scheduler.Process(_configurations, _states, cancellationToken);

        return base.OnExecute(cancellationToken);
    }
}
