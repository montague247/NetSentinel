using NetSentinel.ArgumentHandling;
using Serilog;

namespace NetSentinel
{
    public static class ArgumentProcessor
    {
        public static IArgumentHandler[] Process(params string[] arguments)
        {
            var factory = new ArgumentHandlerFactory();
            var handlers = new List<IArgumentHandler>();
            var index = 0;

            foreach (var argument in arguments)
            {
                var handler = factory.GetHandler(argument);

                if (handler == null)
                {
                    Log.Warning("Unknown argument: {Argument}", argument);

                    continue;
                }

                handlers.Add(handler);
                handler.Process(arguments, ref index);
            }

            return [.. handlers];
        }
    }
}
