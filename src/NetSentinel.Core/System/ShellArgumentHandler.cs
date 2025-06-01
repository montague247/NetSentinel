using Charon.System;
using NetSentinel.Argument;

namespace NetSentinel.System
{
    [ArgumentHandler("--shell", "Execute a shell command or script")]
    public sealed class ShellArgumentHandler : ArgumentHandlerBase
    {
        [ArgumentHandlerProperty("--verbose", "Enables verbose logging")]
        public bool Verbose { get; private set; }

        [ArgumentHandlerProperty("--sudo", "Execute the followed fileName and arguments as root")]
        [ArgumentHandlerProperty("--bash", "Execute the followed command, fileName and arguments")]
        [ArgumentHandlerPropertyValue("command", "echo", "The command to execute in bash")]
        public string? Command { get; private set; }

        public string? FileName { get; private set; }

        public List<string>? Arguments { get; private set; }

        public override void Execute(IGlobalOptions options)
        {
            if (FileName == null ||
                Arguments == null)
                return;

            if (Command == null)
                Shell.Execute(FileName, Arguments, Verbose);
            else
                Shell.BashExecute(Command, FileName, Arguments, Verbose);
        }

        protected override bool Process(string argument, string[] arguments, ref int index)
        {
            switch (argument)
            {
                case "--verbose":
                    Verbose = true;
                    return true;
                case "--sudo":
                    Command = "sudo";
                    index++;
                    break;
                case "--bash":
                    Command = arguments[++index];
                    index++;
                    break;
            }

            FileName = arguments[index++];
            Arguments = CreateArguments(arguments, index);
            index = arguments.Length;

            // return false, because there is nothing else to process

            return false;
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
