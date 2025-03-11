using NetSentinel.RrdTool;

namespace NetSentinel.Core.Tests.RrdTool
{
    public sealed class RrdToolExecuteTests
    {
        [Fact]
        public void Create()
        {
            var fileName = Path.GetFullPath($"{nameof(Create)}.rrd");

            RrdToolExecute.Create(c => c.FileName(fileName));

            Assert.True(File.Exists(fileName));
        }

        [Fact]
        public void Create4RoundRobinArchives()
        {
            var fileName = Path.GetFullPath($"{nameof(Create4RoundRobinArchives)}.rrd");

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
            var fileName = Path.GetFullPath($"{nameof(Create1MinuteDefault)}.rrd");

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
