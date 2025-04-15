using Iot.Device.Ssd13xx.Commands.Ssd1306Commands;
using NetSentinel.SpeedTest;

namespace NetSentinel.Core.Tests.SpeedTest
{
    public class SpeedTestClientTests
    {
        [Fact]
        public void UpdateMaxMbit()
        {
            var client = new SpeedTestClient();
            var server = client.GetSettings().Servers.OrderBy(s => s.Distance).First();
            Assert.NotNull(server);

            var speed = client.TestDownloadSpeed(server, 10);
            Assert.NotEqual(0, speed);
        }
    }
}
