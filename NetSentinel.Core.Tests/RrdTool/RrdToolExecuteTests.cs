using NetSentinel.RrdTool;

namespace NetSentinel.Core.Tests.RrdTool
{
    public sealed class RrdToolExecuteTests
    {
        [Fact]
        public void Create()
        {
            var fileName = Path.GetFullPath("test.rrd");

            RrdToolExecute.Create(c => c.FileName(fileName));

            Assert.True(File.Exists(fileName));
        }
    }
}
