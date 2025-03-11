using System.Reflection;
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

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                var handler = factory.GetHandler(argument);

                if (handler == null)
                {
                    Log.Warning("Unknown argument: {Argument}", argument);

                    continue;
                }

                handlers.Add(handler);

                index++;
                handler.Process(arguments, ref index);
                i = index - 1;
            }

            return [.. handlers];
        }

        public static void Execute(params IArgumentHandler[] handlers)
        {
            foreach (var handler in handlers)
            {
                handler.Execute();
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
