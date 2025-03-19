
namespace NetSentinel.ArgumentHandling
{
    public abstract class ArgumentHandlerBase : IArgumentHandler
    {
        protected abstract Dictionary<string, string> Help { get; }

        public abstract void Execute();

        public virtual void Process(string[] arguments, ref int index)
        {
            while (index < arguments.Length)
            {
                if (!Process(arguments[index], arguments, ref index))
                    return;

                index++;
            }
        }

        public void GenerateHelp(int indent)
        {
            var space = string.Empty.PadRight(indent, '\t');

            foreach (var help in Help)
            {
                Console.Out.WriteLine($"{space}{help.Key} > {help.Value}");
            }
        }

        protected virtual bool Process(string argument, string[] arguments, ref int index)
        {
            return false;
        }
    }
}
