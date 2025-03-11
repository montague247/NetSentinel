using System.Globalization;

namespace NetSentinel.RrdTool
{
    public sealed class DataSourceBuilder
    {
        private string _name = "values";
        private DataSourceType _type;
        private string _heartbeat = "300";
        private string _min = "0";
        private string _max = "U";

        public DataSourceBuilder Name(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));

            return this;
        }

        public DataSourceBuilder Type(DataSourceType type)
        {
            _type = type;

            return this;
        }

        public DataSourceBuilder Heartbeat(int heartbeat)
        {
            if (heartbeat < 1)
                throw new InvalidOperationException("Heartbeat must be greater than or equal to 1");

            _heartbeat = heartbeat.ToString(CultureInfo.InvariantCulture);

            return this;
        }

        public DataSourceBuilder Heartbeat(string heartbeat)
        {
            _heartbeat = heartbeat ?? throw new ArgumentNullException(nameof(heartbeat));

            return this;
        }

        public DataSourceBuilder Min(double min)
        {
            _min = min.ToString(CultureInfo.InvariantCulture);

            return this;
        }

        public DataSourceBuilder Min(string min)
        {
            _min = min ?? throw new ArgumentNullException(nameof(min));

            return this;
        }

        public DataSourceBuilder Max(double max)
        {
            _max = max.ToString(CultureInfo.InvariantCulture);

            return this;
        }

        public DataSourceBuilder Max(string max)
        {
            _max = max ?? throw new ArgumentNullException(nameof(max));

            return this;
        }

        internal string Build()
        {
            if (_name == null)
                throw new InvalidOperationException("Name is required");

            return $"DS:{_name}:{_type.ToString().ToUpper()}:{_heartbeat}:{_min}:{_max}";
        }
    }
}
