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

        public static void Graph(Action<GraphBuilder> graph)
        {
            var builder = new GraphBuilder();
            graph(builder);

            Execute(builder.Build());
        }

        private static void Execute(List<string> arguments)
        {
#if DEBUG
            Console.Out.WriteLine($"rrdtool {string.Join(" ", arguments)}");
#endif

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

            // Starte paralleles Lesen von Standard-Output und Standard-Error
            var outputTask = ReadStreamAsync(process.StandardOutput, Console.Out);
            var errorTask = ReadStreamAsync(process.StandardError, Console.Error);

            Task.WhenAll(outputTask, errorTask); // Warten auf beide Streams

            var timeout = TimeSpan.FromMinutes(1);

            if (!process.WaitForExit(timeout))
            {
                process.Close();
                throw new InvalidOperationException($"RrdTool timeout after {timeout}");
            }

            if (process.ExitCode != 0)
                throw new InvalidOperationException($"RrdTool error (Exit Code: {process.ExitCode})");
        }

        private static async Task ReadStreamAsync(StreamReader stream, TextWriter writer)
        {
            string? line;

            while ((line = stream.ReadLine()) != null)
            {
                await writer.WriteLineAsync(line); // Sofortiges Schreiben in die Konsole
            }
        }
    }
}
