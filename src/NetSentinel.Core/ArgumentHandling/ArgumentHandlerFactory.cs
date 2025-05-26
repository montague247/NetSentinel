using System.Reflection;

namespace NetSentinel.ArgumentHandling
{
    public sealed class ArgumentHandlerFactory
    {
        private readonly Dictionary<string, Type> _handlers = [];

        public ArgumentHandlerFactory()
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(s => typeof(IArgumentHandler).IsAssignableFrom(s) && !s.IsInterface && !s.IsAbstract);

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<ArgumentHandlerAttribute>();

                if (attribute == null)
                    continue;

                _handlers.Add(attribute.Name ?? string.Empty, type);
            }
        }

        public IReadOnlyDictionary<string, Type> Handlers { get { return _handlers; } }

        public IArgumentHandler? GetHandler(string argument)
        {
            if (_handlers.TryGetValue(argument, out var type))
                return (IArgumentHandler?)Activator.CreateInstance(type);

            return default;
        }
    }
}
