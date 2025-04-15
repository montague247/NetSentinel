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
    }
}
