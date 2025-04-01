using NetSentinel;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
    
var handlers = ArgumentProcessor.Process(out IGlobalOptions options, args);

if (handlers.Length == 0)
    ArgumentProcessor.GenerateHelp();
else
    ArgumentProcessor.Execute(options, handlers);
