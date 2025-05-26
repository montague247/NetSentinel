using NetSentinel.SpeedTest;

namespace NetSentinel.Core.Tests
{
    public class ArgumentProcessorTests
    {
        [Fact]
        public void SpeedTestMaxMbit()
        {
            var args = new string[] { "--speed-test", "--max-mbit", "10" };
            var handlers = ArgumentProcessor.Process(out _, args);
            Assert.Single(handlers);
            Assert.IsType<ArgumentHandler>(handlers[0]);
        }

        [Fact]
        public void DiscoveryWithGlobalOptions()
        {
            var args = new string[] { "--sudo-alt", "--discovery" };
            var handlers = ArgumentProcessor.Process(out IGlobalOptions options, args);
            Assert.True(options.SudoAlternative);
            Assert.Single(handlers);
            Assert.IsType<Discovery.ArgumentHandler>(handlers[0]);
        }

        [Fact]
        public void HelpCommand()
        {
            var args = new string[] { "--help" };
            var handlers = ArgumentProcessor.Process(out _, args);
            Assert.Empty(handlers);
            // The help command should not return any handlers, it just prints help information.
        }

        [Fact]
        public void GenerateHelp()
        {
            var sw = new StringWriter();
            // Redirect console output to a StringWriter to capture the help output.
            Console.SetOut(sw);

            ArgumentProcessor.GenerateHelp();

            var output = sw.ToString();
            Assert.Contains("Available arguments:", output);
            Assert.Contains("Global options for NetSentinel", output);
            Assert.Contains("  --sudo-alt   - Use sudo alternative", output);
            Assert.Contains("  --no-install - Do not install missing packages", output);
            Assert.Contains("  --help       - Show this help message", output);
            Assert.Contains("Specific options:", output);
            Assert.Contains("  --cpu-temperature - Read CPU temperature", output);
            Assert.Contains("  --discovery - Run discovery scan", output);
            Assert.Contains("    --ip {ipOrRange}  - IP or IP range (comma-separated)", output);
            Assert.Contains("      ipOrRange - 192.168.1.1/20: Default IP range for ARP ping scan", output);
            Assert.Contains("      ipOrRange - 192.168.4.1: Alternative IP for ARP ping scan", output);
            Assert.Contains("    --keep-temp-files - Keeps all temporary files", output);
            // Add more assertions as needed to check for other arguments.
            Console.SetOut(Console.Out); // Reset console output to default.
        }
    }
}
