using NetSentinel.Nmap.Types;

namespace NetSentinel.Core.Tests.Nmap
{
    public sealed class XmlTests
    {
        [Fact]
        public void Scanned_2()
        {
            var path = Path.Combine(TestExtensions.GetCallerPath()!, "examples", "xml", "scanned_2.xml");
            var nmaprun = File.ReadAllText(path).FromXml<NmapRun>();

            Assert.NotNull(nmaprun);
        }

        [Fact]
        public void Scanned_4()
        {
            var path = Path.Combine(TestExtensions.GetCallerPath()!, "examples", "xml", "scanned_4.xml");
            var nmaprun = File.ReadAllText(path).FromXml<NmapRun>();

            Assert.NotNull(nmaprun);
        }
    }
}
