using System.Globalization;

namespace NetSentinel.RrdTool
{
    public sealed class RoundRobinArchiveBuilder
    {
        private ConsolidationFunction _consolidationFunction;
        private double _xff = 0.5;
        private int _steps = 1;
        private int _rows = 3600;

        public RoundRobinArchiveBuilder ConsolidationFunction(ConsolidationFunction consolidationFunction)
        {
            _consolidationFunction = consolidationFunction;

            return this;
        }

        public RoundRobinArchiveBuilder Xff(double xff)
        {
            _xff = xff;

            return this;
        }

        public RoundRobinArchiveBuilder Steps(int steps)
        {
            _steps = steps;

            return this;
        }

        public RoundRobinArchiveBuilder Rows(int rows)
        {
            _rows = rows;

            return this;
        }

        internal string Build()
        {
            return $"RRA:{_consolidationFunction.ToString().ToUpper()}:{_xff.ToString(CultureInfo.InvariantCulture)}:{_steps}:{_rows}";
        }
    }
}
