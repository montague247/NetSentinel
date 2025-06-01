using NetSentinel.Argument;

namespace NetSentinel
{
    [ArgumentHandler(null, "Global options for NetSentinel")]
    public sealed class GlobalArgumentHandler : ArgumentHandlerBase, IGlobalOptions
    {
        [ArgumentHandlerProperty("--sudo-alt", "Use sudo alternative")]
        public bool SudoAlternative { get; private set; }

        [ArgumentHandlerProperty("--no-install", "Do not install missing packages")]
        public bool NoInstall { get; private set; }

        [ArgumentHandlerProperty("--help", "Show this help message")]
        public bool Help { get; private set; }

        public override void Execute(IGlobalOptions options)
        {
            // nothing to do
        }

        protected override bool Process(string argument, string[] arguments, ref int index)
        {
            switch (argument)
            {
                case "--sudo-alt":
                    SudoAlternative = true;
                    return true;
                case "--no-install":
                    NoInstall = true;
                    return true;
                default:
                    return false;
            }
        }
    }
}
