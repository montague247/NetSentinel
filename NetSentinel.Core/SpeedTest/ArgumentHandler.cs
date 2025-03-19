using System.Globalization;
using NetSentinel.ArgumentHandling;
using NetSentinel.SpeedTest.Types;

namespace NetSentinel.SpeedTest
{
    [ArgumentHandler("--speed-test")]
    public sealed class ArgumentHandler : ArgumentHandlerBase
    {
        private double _maxMbit = 1024;

        protected override Dictionary<string, string> Help => new()
        {
            { "--max-mbit", "Maximum speed in Mbit/s" }
        };

        public override void Execute()
        {
            var client = new SpeedTestClient();
            var settings = client.GetSettings();

            var servers = SelectServers(client, settings);
            var bestServer = SelectBestServer(servers);

            Console.WriteLine($"Testing speed with max {_maxMbit} Mbit...");

            var downloadSpeed = client.TestDownloadSpeed(bestServer, _maxMbit, settings?.Download?.ThreadsPerUrl ?? 2);
            PrintSpeed("Download", downloadSpeed);

            var uploadSpeed = client.TestUploadSpeed(bestServer, settings?.Upload?.ThreadsPerUrl ?? 2);
            PrintSpeed("Upload", uploadSpeed);
        }

        protected override bool Process(string argument, string[] arguments, ref int index)
        {
            switch (argument)
            {
                case "--max-mbit":
                    _maxMbit = double.Parse(arguments[++index], CultureInfo.InvariantCulture);
                    return true;
                default:
                    return false;
            }
        }

        private static Server SelectBestServer(IEnumerable<Server> servers)
        {
            Console.WriteLine();
            Console.WriteLine("Best server by latency:");

            var bestServer = servers.OrderBy(x => x.Latency).First();

            PrintServerDetails(bestServer);
            Console.WriteLine();

            return bestServer;
        }

        private static List<Server> SelectServers(SpeedTestClient client, Settings settings)
        {
            Console.WriteLine("Selecting best server by distance...");

            var servers = settings.Servers.OrderBy(s => s.Distance).Take(10).ToList();

            foreach (var server in servers)
            {
                server.Latency = client.TestServerLatency(server);
                PrintServerDetails(server);
            }

            return servers;
        }

        private static void PrintServerDetails(Server server)
        {
            Console.WriteLine("Hosted by {0} ({1}/{2}), distance: {3}km, latency: {4}ms", server.Sponsor, server.Name,
                server.Country, (int)server.Distance / 1000, server.Latency);
        }

        private static void PrintSpeed(string type, double speed)
        {
            if (speed > 1024)
                Console.WriteLine("{0} speed: {1} Mbps", type, Math.Round(speed / 1024, 2));
            else
                Console.WriteLine("{0} speed: {1} Kbps", type, Math.Round(speed, 2));
        }
    }
}
