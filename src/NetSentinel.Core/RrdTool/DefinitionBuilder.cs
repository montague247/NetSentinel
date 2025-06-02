using System.Text;

namespace NetSentinel.RrdTool;

public sealed class DefinitionBuilder
{
    private string _variableName = "dsv";
    private string? _fileName;
    private string _dataSourceName = "values";
    private ConsolidationFunction _consolidationFunction;
    private int? _step;
    private string? _start;
    private string? _end;
    private ConsolidationFunction? _reduce;

    public DefinitionBuilder()
    {
    }

    public DefinitionBuilder(string fileName, ConsolidationFunction consolidationFunction, int? step, string? start, string? end, ConsolidationFunction? reduce)
    {
        _fileName = fileName;
        _consolidationFunction = consolidationFunction;
        _step = step;
        _start = start;
        _end = end;
        _reduce = reduce;
    }

    public DefinitionBuilder(string variableName, string fileName, string dataSourceName, ConsolidationFunction consolidationFunction, int? step, string? start, string? end, ConsolidationFunction? reduce)
        : this(fileName, consolidationFunction, step, start, end, reduce)
    {
        _variableName = variableName;
        _dataSourceName = dataSourceName;
    }

    public DefinitionBuilder VariableName(string variableName)
    {
        _variableName = variableName ?? throw new ArgumentNullException(nameof(variableName));

        return this;
    }

    public DefinitionBuilder FileName(string fileName)
    {
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        return this;
    }

    public DefinitionBuilder DataSourceName(string dataSourceName)
    {
        _dataSourceName = dataSourceName ?? throw new ArgumentNullException(nameof(dataSourceName));

        return this;
    }

    public DefinitionBuilder ConsolidationFunction(ConsolidationFunction consolidationFunction)
    {
        _consolidationFunction = consolidationFunction;

        return this;
    }

    public DefinitionBuilder Step(int step)
    {
        if (step <= 0)
            throw new ArgumentOutOfRangeException(nameof(step), "Step must be greater than zero");

        _step = step;

        return this;
    }

    public DefinitionBuilder Start(string start)
    {
        _start = start ?? throw new ArgumentNullException(nameof(start));

        return this;
    }

    public DefinitionBuilder End(string end)
    {
        _end = end ?? throw new ArgumentNullException(nameof(end));

        return this;
    }

    public DefinitionBuilder Reduce(ConsolidationFunction reduce)
    {
        _reduce = reduce;

        return this;
    }

    internal string Build()
    {
        if (_fileName == null)
            throw new InvalidOperationException("FileName is required");

        var sb = new StringBuilder();

        sb.Append($"DEF:{_variableName}={_fileName}:{_dataSourceName}:{_consolidationFunction.ToString().ToUpper()}");

        if (_step.HasValue)
            sb.Append($":step={_step.Value}");

        if (!string.IsNullOrEmpty(_start))
            sb.Append($":start={_start}");

        if (!string.IsNullOrEmpty(_end))
            sb.Append($":end={_end}");

        if (_reduce.HasValue)
            sb.Append($":reduce={_reduce.Value.ToString().ToUpper()}");

        return sb.ToString();
    }
}
