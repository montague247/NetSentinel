using Charon.Core.Tests;
using NetSentinel.RrdTool;

namespace NetSentinel.Core.Tests.RrdTool
{
    public sealed class RrdToolExecuteTests
    {
        [SkipOnContinuousIntegrationFact]
        public void CreateSimple()
        {
            var fileName = Path.GetFullPath("Simple.rrd");

            RrdToolExecute.Create(c => c.FileName(fileName));

            Assert.True(File.Exists(fileName));
        }

        [SkipOnContinuousIntegrationFact]
        public void UpdateSimple()
        {
            var fileName = Path.GetFullPath("Simple.rrd");

            if (!File.Exists(fileName))
                CreateSimple();

            RrdToolExecute.Update(u => u
                                    .FileName(fileName)
                                    .Value(Math.Round(new Random().NextDouble() * 100, 2))
                                );

            Assert.True(File.Exists(fileName));
        }

        [SkipOnContinuousIntegrationFact]
        public void GraphSimple()
        {
            UpdateSimple();

            var fileName = Path.GetFullPath("Simple.rrd");

            RrdToolExecute.Graph(g => g
                                    .FileName($"{fileName}.png")
                                    .Definition(fileName)
                                    .Line(1, "dsv")
                                );

            Assert.True(File.Exists(fileName));
        }

        [SkipOnContinuousIntegrationFact]
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

        [SkipOnContinuousIntegrationFact]
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
