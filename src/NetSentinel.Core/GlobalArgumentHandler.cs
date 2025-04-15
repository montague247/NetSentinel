using NetSentinel.ArgumentHandling;

namespace NetSentinel
{
    public sealed class GlobalArgumentHandler : ArgumentHandlerBase, IGlobalOptions
    {
        public bool SudoAlternative { get; private set; }

        protected override Dictionary<string, string> Help => new()
        {
            { "--sudo-alt", "Use sudo alternative" }
        };

        public override void Execute(IGlobalOptions options)
        {
            // nothing to do
        }

        protected override bool Process(string argument, string[] arguments, ref int index)
        {
            switch (argument)
            {
                case "--sudo-alt":
                    SudoAlternative=true;
                    return true;
                default:
                    return false;
            }
        }
    }
}
