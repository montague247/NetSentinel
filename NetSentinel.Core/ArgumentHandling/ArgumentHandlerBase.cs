namespace NetSentinel.ArgumentHandling
{
    public abstract class ArgumentHandlerBase : IArgumentHandler
    {
        protected abstract Dictionary<string, string> Help { get; }

        public abstract void Execute();

        public abstract void Process(string[] arguments, ref int index);

        public void GenerateHelp(int indent)
        {
            var space = string.Empty.PadRight(indent, '\t');

            foreach (var help in Help)
            {
                Console.Out.WriteLine($"{space}{help.Key} > {help.Value}");
            }
        }
    }
}
