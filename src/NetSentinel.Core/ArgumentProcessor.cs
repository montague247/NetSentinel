using System.Reflection;
using NetSentinel.ArgumentHandling;
using Serilog;

namespace NetSentinel
{
    public static class ArgumentProcessor
    {
        public static IArgumentHandler[] Process(out IGlobalOptions options, params string[] arguments)
        {
            var factory = new ArgumentHandlerFactory();
            var handlers = new List<IArgumentHandler>();
            var globalHandler = new GlobalArgumentHandler();

            for (int index = 0; index < arguments.Length; index++)
            {
                if (index == 0)
                    globalHandler.Process(arguments, ref index);

                var argument = arguments[index++];
                var handler = factory.GetHandler(argument);

                if (handler == null)
                {
                    Log.Warning("Unknown argument: {Argument}", argument);

                    continue;
                }

                handlers.Add(handler);

                handler.Process(arguments, ref index);
                index--;
            }

            options = globalHandler;

            return [.. handlers];
        }

        public static void Execute(IGlobalOptions options, params IArgumentHandler[] handlers)
        {
            foreach (var handler in handlers)
            {
                handler.Execute(options);
            }
        }

        public static void GenerateHelp()
        {
            var factory = new ArgumentHandlerFactory();

            Console.WriteLine("Available arguments:");

            foreach (var handler in factory.Handlers)
            {
                var attribute = handler.Value.GetCustomAttribute<ArgumentHandlerAttribute>();

                if (attribute == null)
                    continue;

                Console.WriteLine(attribute.Name);

                var instance = (IArgumentHandler?)Activator.CreateInstance(handler.Value);
                instance?.GenerateHelp(1);
            }
        }
    }
}
