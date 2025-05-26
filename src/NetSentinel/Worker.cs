using Charon.Hosting;
using Microsoft.Extensions.Hosting;

namespace NetSentinel;

public sealed class Worker(IHostApplicationLifetime hostApplicationLifetime) : WorkerBase(hostApplicationLifetime)
{
    protected override Task OnStart(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected override Task OnStop(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
