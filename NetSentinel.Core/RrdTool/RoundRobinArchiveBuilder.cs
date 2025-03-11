using System.Globalization;

namespace NetSentinel.RrdTool
{
    public sealed class RoundRobinArchiveBuilder
    {
        private ConsolidationFunction _consolidationFunction;
        private double _xff = 0.5;
        private string _steps = "1";
        private string _rows = "3600";

        public RoundRobinArchiveBuilder ConsolidationFunction(ConsolidationFunction consolidationFunction)
        {
            _consolidationFunction = consolidationFunction;

            return this;
        }

        public RoundRobinArchiveBuilder Xff(double xff)
        {
            if (_xff < 0)
                throw new InvalidOperationException("XFF must be greater than or equal to 0");
            if (_xff >= 1)
                throw new InvalidOperationException("XFF must be less than 1");

            _xff = xff;

            return this;
        }

        public RoundRobinArchiveBuilder Steps(int steps)
        {
            if (steps < 1)
                throw new InvalidOperationException("Steps must be greater than or equal to 1");

            _steps = steps.ToString(CultureInfo.InvariantCulture);

            return this;
        }

        public RoundRobinArchiveBuilder Steps(string steps)
        {
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));

            return this;
        }

        public RoundRobinArchiveBuilder Rows(int rows)
        {
            if (rows < 1)
                throw new InvalidOperationException("Rows must be greater than or equal to 1");

            _rows = rows.ToString(CultureInfo.InvariantCulture);

            return this;
        }

        public RoundRobinArchiveBuilder Rows(string rows)
        {
            _rows = rows ?? throw new ArgumentNullException(nameof(rows));

            return this;
        }

        internal string Build()
        {
            return $"RRA:{_consolidationFunction.ToString().ToUpper()}:{_xff.ToString(CultureInfo.InvariantCulture)}:{_steps}:{_rows}";
        }
    }
}
