using NetSentinel.ArgumentHandling;

namespace NetSentinel
{
    [ArgumentHandler("--shell")]
    public sealed class ShellArgumentHandler : ArgumentHandlerBase
    {
        private bool _verbose;
        private string? _command;
        private string? _fileName;
        private List<string>? _arguments;

        protected override Dictionary<string, string> Help => new()
        {
            { "--verbose", "Enables verbose logging" },
            { "--sudo", "Execute the followed fileName and arguments as root" },
            { "--bash", "Execute the followed command, fileName and arguments" }
        };

        public override void Execute()
        {
            if (_fileName == null ||
                _arguments == null)
                return;

            if (_command == null)
                Shell.Execute(_fileName, _arguments, _verbose);
            else
                Shell.BashExecute(_command, _fileName, _arguments, _verbose);
        }

        public override void Process(string[] arguments, ref int index)
        {
            if (index >= arguments.Length)
                return;

            if (string.Compare(arguments[index], "--verbose", StringComparison.Ordinal) == 0)
            {
                index++;
                _verbose = true;
            }

            switch (arguments[index])
            {
                case "--sudo":
                    _command = "sudo";
                    index++;
                    break;
                case "--bash":
                    _command = arguments[++index];
                    index++;
                    break;
            }

            _fileName = arguments[index++];
            _arguments = CreateArguments(arguments, index);
            index = arguments.Length;
        }

        private static List<string> CreateArguments(string[] arguments, int index)
        {
            var list = new List<string>();

            for (int i = index; i < arguments.Length; i++)
            {
                list.Add(arguments[i]);
            }

            return list;
        }
    }
}
