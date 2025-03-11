using NetSentinel.RrdTool;

namespace NetSentinel.Core.Tests.RrdTool
{
    public sealed class RrdToolExecuteTests
    {
        [Fact]
        public void CreateSimple()
        {
            var fileName = Path.GetFullPath($"Simple.rrd");

            RrdToolExecute.Create(c => c.FileName(fileName));

            Assert.True(File.Exists(fileName));
        }

        [Fact]
        public void UpdateSimple()
        {
            var fileName = Path.GetFullPath($"Simple.rrd");

            if (!File.Exists(fileName))
                CreateSimple();

            RrdToolExecute.Update(u => u
                                    .FileName(fileName)
                                    .Value(new Random().NextDouble() * 100)
                                );

            Assert.True(File.Exists(fileName));
        }

        [Fact]
        public void Create4RoundRobinArchives()
        {
            var fileName = Path.GetFullPath("4RoundRobinArchives.rrd");

            RrdToolExecute.Create(c => c
                                    .FileName(fileName)
                                    .Start("now-2h")
                                    .Step("1s")
                                    .DataSource(d => d
                                        .Heartbeat("5m")
                                        .Max(24000)
                                    )
                                    .RoundRobinArchive(r => r
                                        .ConsolidationFunction(ConsolidationFunction.Average)
                                        .Steps("1s")
                                        .Rows("10d")
                                    )
                                    .RoundRobinArchive(r => r
                                        .ConsolidationFunction(ConsolidationFunction.Average)
                                        .Steps("1m")
                                        .Rows("90d")
                                    )
                                    .RoundRobinArchive(r => r
                                        .ConsolidationFunction(ConsolidationFunction.Average)
                                        .Steps("1h")
                                        .Rows("18M")
                                    )
                                    .RoundRobinArchive(r => r
                                        .ConsolidationFunction(ConsolidationFunction.Average)
                                        .Steps("1d")
                                        .Rows("10y")
                                    )
                                );

            Assert.True(File.Exists(fileName));
        }

        [Fact]
        public void Create1MinuteDefault()
        {
            var fileName = Path.GetFullPath("1MinuteDefault.rrd");

            RrdToolExecute.Create(c => c
                                    .FileName(fileName)
                                    .Step("1m")
                                    .DataSource(d => d
                                        .Heartbeat("5m")
                                    )
                                    .RoundRobinArchive(r => r
                                        .ConsolidationFunction(ConsolidationFunction.Average)
                                        .Steps("1m")
                                        .Rows("90d")
                                    )
                                    .RoundRobinArchive(r => r
                                        .ConsolidationFunction(ConsolidationFunction.Average)
                                        .Steps("1h")
                                        .Rows("18M")
                                    )
                                    .RoundRobinArchive(r => r
                                        .ConsolidationFunction(ConsolidationFunction.Average)
                                        .Steps("1d")
                                        .Rows("10y")
                                    )
                                );

            Assert.True(File.Exists(fileName));
        }
    }
}
