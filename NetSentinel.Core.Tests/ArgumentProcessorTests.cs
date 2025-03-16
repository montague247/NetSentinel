using NetSentinel.SpeedTest;

namespace NetSentinel.Core.Tests
{
    public class ArgumentProcessorTests
    {
        [Fact]
        public void SpeedTestMaxMbit()
        {
            var args = new string[] { "--speed-test", "--max-mbit", "10" };
            var handlers = ArgumentProcessor.Process(args);
            Assert.Single(handlers);
            Assert.IsType<ArgumentHandler>(handlers[0]);
        }
    }
}
