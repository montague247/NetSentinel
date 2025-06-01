using NetSentinel.Argument;

namespace NetSentinel.RrdTool
{
    [ArgumentHandler("--rrdtool-test", "Run RRDTool test")]
    public sealed class ArgumentHandler : IArgumentHandler
    {
        private const string Name = "rrdtool-test";
        private const string RrdFileName = $"{Name}.rrd";
        private const string GraphFileName = $"{Name}.png";

        public void Execute(IGlobalOptions globalOptions)
        {
            if (!File.Exists(RrdFileName))
            {
                Console.WriteLine($"Create RRD file: {RrdFileName}");

                RrdToolExecute.Create(c => c
                    .FileName(RrdFileName)
                    .Step(1)
                    .DataSource(d => d
                        .Heartbeat(2)
                        .Min(0)
                        .Max(100)
                    )
                , globalOptions);
            }

            var updateCount = 0;
            var rnd = new Random();

            do
            {
                var value = rnd.NextDouble() * 100;

                RrdToolExecute.Update(u => u
                    .FileName(RrdFileName)
                    .Value(value)
                , globalOptions);

                if (++updateCount == 5)
                {
                    updateCount = 0;

                    Console.WriteLine($"Create RRD graph: {GraphFileName}");

                    RrdToolExecute.Graph(g => g
                        .FileName(GraphFileName)
                        .Start("now-1h")
                        .LowerLimit(0)
                        .UpperLimit(100)
                        .Definition(RrdFileName)
                        .Line(1, "dsv")
                    , globalOptions);
                }

                Thread.Sleep(1000);
            } while (true);
        }

        public void Process(string[] arguments, ref int index)
        {
            // nothing to do
        }

        public void GenerateHelp(int indent)
        {
            // nothing to do
        }
    }
}
