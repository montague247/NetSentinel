using NetSentinel;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Debug("{Name} loaded", typeof(NetSentinel.Automation.ZigbeeGateway).Assembly.GetName().Name);

var handlers = ArgumentProcessor.Process(out IGlobalOptions options, args);

if (handlers.Length == 0)
    ArgumentProcessor.GenerateHelp();
else
    ArgumentProcessor.Execute(options, handlers);
