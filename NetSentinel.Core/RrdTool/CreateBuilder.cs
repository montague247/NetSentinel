namespace NetSentinel.RrdTool
{
    public sealed class CreateBuilder : ArgumentsBuilder
    {
        private string? _fileName;
        private string? _startTime;
        private string? _step;
        private List<DataSourceBuilder> _dataSources = [];
        private List<RoundRobinArchiveBuilder> _archives = [];

        public CreateBuilder FileName(string fileName)
        {
            _fileName = fileName;

            return this;
        }

        public CreateBuilder StartTime(string startTime)
        {
            _startTime = startTime;

            return this;
        }

        public CreateBuilder Step(string step)
        {
            _step = step;

            return this;
        }

        public CreateBuilder DataSource(Action<DataSourceBuilder> dataSource)
        {
            var builder = new DataSourceBuilder();
            dataSource(builder);

            _dataSources.Add(builder);

            return this;
        }

        public CreateBuilder RoundRobinArchive(Action<RoundRobinArchiveBuilder> archive)
        {
            var builder = new RoundRobinArchiveBuilder();
            archive(builder);

            _archives.Add(builder);

            return this;
        }

        internal List<string> Build()
        {
            var arguments = new List<string>();

            if (_fileName == null)
                throw new InvalidOperationException("FileName is required");

            Add(arguments, "create", _fileName);
            Add(arguments, "-b", _startTime);
            Add(arguments, "-s", _step);

            if (_dataSources.Count == 0)
                arguments.Add(new DataSourceBuilder().Build());
            else
            {
                foreach (var dataSource in _dataSources)
                {
                    arguments.Add(dataSource.Build());
                }
            }

            if (_archives.Count == 0)
                arguments.Add(new RoundRobinArchiveBuilder().Build());
            else
            {
                foreach (var archive in _archives)
                {
                    arguments.Add(archive.Build());
                }
            }

            return arguments;
        }
    }
}
