using System.Globalization;

namespace NetSentinel.RrdTool;

public sealed class CreateBuilder : ArgumentsBuilder
{
    private string? _fileName;
    private string? _start;
    private string? _step;
    private readonly List<DataSourceBuilder> _dataSources = [];
    private readonly List<RoundRobinArchiveBuilder> _archives = [];

    public CreateBuilder FileName(string fileName)
    {
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        return this;
    }

    public CreateBuilder Start(string start)
    {
        _start = start ?? throw new ArgumentNullException(nameof(start));

        return this;
    }

    public CreateBuilder Step(int step)
    {
        if (step < 1)
            throw new InvalidOperationException("Step must be greater than or equal to 1");

        _step = step.ToString(CultureInfo.InvariantCulture);

        return this;
    }

    public CreateBuilder Step(string step)
    {
        _step = step ?? throw new ArgumentNullException(nameof(step));

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
        Add(arguments, "-b", _start);
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
