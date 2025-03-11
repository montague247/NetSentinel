using System.Diagnostics;

namespace NetSentinel.RrdTool
{
    public static class RrdToolExecute
    {
        /// <summary>
        /// Creates an RRD (Round-Robin Database) using the specified builder action.
        /// </summary>
        /// <param name="create">An action that configures the <see cref="CreateBuilder"/> instance.</param>
        public static void Create(Action<CreateBuilder> create)
        {
            var builder = new CreateBuilder();
            create(builder);

            Execute(builder.Build());
        }

        public static void Update(Action<UpdateBuilder> update)
        {
            var builder = new UpdateBuilder();
            update(builder);

            Execute(builder.Build());
        }

        private static void Execute(List<string> arguments)
        {
            Console.WriteLine($"rrdtool {string.Join(" ", arguments)}");

            var psi = new ProcessStartInfo
            {
                FileName = "rrdtool",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            foreach (var argument in arguments)
            {
                psi.ArgumentList.Add(argument);
            }

            var process = new Process { StartInfo = psi };

            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                throw new InvalidOperationException($"RrdTool error: {error}");
            }
        }
    }
}
