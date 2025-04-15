using System.Diagnostics;
using Serilog;

namespace NetSentinel
{
    public static class Shell
    {
        public static void Execute(string fileName, List<string> arguments, bool verbose = false, bool shellExecute = false)
        {
            if (verbose)
                Log.Information("Execute: {FileName} {Arguments}", fileName, string.Join(' ', arguments));
            else
                Log.Debug("Execute: {FileName} {Arguments}", fileName, string.Join(' ', arguments));

            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = shellExecute,
                CreateNoWindow = !shellExecute
            };

            foreach (var argument in arguments)
            {
                psi.ArgumentList.Add(argument);
            }

            var process = new Process { StartInfo = psi };

            process.Start();

            // Start parallel reading of Standard-Output and Standard-Error
            var outputTask = Task.Run(() => ReadStreamAsync(process.StandardOutput, false));
            var errorTask = Task.Run(() => ReadStreamAsync(process.StandardError, true));

            Task.WhenAll(outputTask, errorTask);

            var timeout = TimeSpan.FromMinutes(60);

            if (!process.WaitForExit(timeout))
            {
                process.Close();
                throw new InvalidOperationException($"{fileName} timeout after {timeout}");
            }

            if (process.ExitCode != 0)
                throw new InvalidOperationException($"{fileName} error (Exit Code: {process.ExitCode})");
        }

        public static void BashExecute(string command, string fileName, List<string> arguments, bool verbose = false)
        {
            var bashArguments = new List<string> { command, fileName };
            bashArguments.AddRange(arguments);

            // Arguments = string.Format("-c \"sudo {0} {1} {2}\"", "/path/to/script", "arg1", arg2)
            Execute("/bin/bash", ["-c", string.Join(' ', bashArguments)], verbose);
        }

        public static void SudoExecute(string fileName, List<string> arguments, IGlobalOptions options, bool verbose = false)
        {
            if (options.SudoAlternative)
            {
                var sudoArguments = new List<string> { fileName };
                sudoArguments.AddRange(arguments);

                Execute("sudo", sudoArguments, verbose);
            }
            else
                BashExecute("sudo", fileName, arguments, verbose);
        }

        private static void ReadStreamAsync(StreamReader stream, bool error)
        {
            string? line;

            while ((line = stream.ReadLine()) != null)
            {
                if (error)
                    Log.Error(line);
                else
                    Log.Information(line);
            }
        }
    }
}
