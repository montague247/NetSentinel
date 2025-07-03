using Charon.Hosting;
using NetSentinel.Argument;

namespace NetSentinel;

[ArgumentHandler("--service", "Run as service")]
public sealed class ServiceArgumentHandler : ArgumentHandlerBase, IServiceOptions
{
    [ArgumentHandlerProperty("--config", "Path to the configuration file")]
    [ArgumentHandlerPropertyValue("NetSentinel.json", "Default configuration file path")]
    [ArgumentHandlerPropertyValue("config.json", "Alternative configuration file path")]
    public string ConfigPath { get; private set; } = "NetSentinel.json";

    public override void Execute(IGlobalOptions options)
    {
        Charon.Hosting.Service.Run<Worker>("NetSentinel", this, Environment.GetCommandLineArgs());
    }

    protected override bool Process(string argument, string[] arguments, ref int index)
    {
        switch (argument)
        {
            case "--config":
                ConfigPath = arguments[++index];
                return true;
        }

        return false;
    }
}
