using Charon.System;
using NetSentinel.Argument;
using Serilog;

namespace NetSentinel.System;

[ArgumentHandler("--cpu-usage", "Returns the CPU usage of the system")]
public sealed class CpuUsageArgumentHandler : ArgumentHandlerBase
{
    public override void Execute(IGlobalOptions options)
    {
        var cpuUsage = SystemInfo.GetCpuUsage().GetAwaiter().GetResult();

        Log.Information("CPU usage: user={CpuUsageUser}%, system={CpuUsageSystem}%", cpuUsage.User, cpuUsage.System);
    }

    protected override bool Process(string argument, string[] arguments, ref int index)
    {
        return false;
    }
}
