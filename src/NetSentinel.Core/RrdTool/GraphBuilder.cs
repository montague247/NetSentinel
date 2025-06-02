using System.Text;

namespace NetSentinel.RrdTool;

public sealed class GraphBuilder : ArgumentsBuilder
{
    private string? _fileName;
    private string? _start;
    private string? _end;
    private int? _step;
    private string? _title;
    private string? _verticalLabel;
    private int? _width;
    private int? _height;
    private bool _onlyGraph;
    private bool _fullSizeMode;
    private int? _upperLimit;
    private int? _lowerLimit;
    private bool _rigid;
    private bool _allowShrink;
    private ImageFormat _imageFormat;
    private bool _disableRrdtoolTag = true;
    // start data section
    private readonly List<DefinitionBuilder> _definitions = [];
    // start graph section
    private List<string>? _prints;
    private List<string>? _gprints;
    private List<string>? _comments;
    private List<string>? _lines;
    private List<string>? _areas;
    private List<string>? _ticks;
    private List<string>? _shifts;
    private List<string>? _textaligns;

    public GraphBuilder FileName(string fileName)
    {
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        return this;
    }

    public GraphBuilder Start(string start)
    {
        _start = start;

        return this;
    }

    public GraphBuilder End(string end)
    {
        _end = end;

        return this;
    }

    public GraphBuilder Step(int step)
    {
        if (step <= 0)
            throw new ArgumentOutOfRangeException(nameof(step), "Step must be greater than zero");

        _step = step;

        return this;
    }

    public GraphBuilder Title(string title)
    {
        _title = title ?? throw new ArgumentNullException(nameof(title));

        return this;
    }

    public GraphBuilder VerticalLabel(string verticalLabel)
    {
        _verticalLabel = verticalLabel ?? throw new ArgumentNullException(nameof(verticalLabel));

        return this;
    }

    public GraphBuilder Width(int width)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero");

        _width = width;

        return this;
    }

    public GraphBuilder Height(int height)
    {
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than zero");

        _height = height;

        return this;
    }

    public GraphBuilder OnlyGraph(bool onlyGraph)
    {
        _onlyGraph = onlyGraph;

        return this;
    }

    public GraphBuilder FullSizeMode(bool fullSizeMode)
    {
        _fullSizeMode = fullSizeMode;

        return this;
    }

    public GraphBuilder UpperLimit(int upperLimit)
    {
        _upperLimit = upperLimit;

        return this;
    }

    public GraphBuilder LowerLimit(int lowerLimit)
    {
        _lowerLimit = lowerLimit;

        return this;
    }

    public GraphBuilder Rigid(bool rigid)
    {
        _rigid = rigid;

        return this;
    }

    public GraphBuilder AllowShrink(bool allowShrink)
    {
        _allowShrink = allowShrink;

        return this;
    }
    public GraphBuilder ImageFormat(ImageFormat imageFormat)
    {
        _imageFormat = imageFormat;

        return this;
    }

    public GraphBuilder DisableRrdtoolTag(bool disableRrdtoolTag)
    {
        _disableRrdtoolTag = disableRrdtoolTag;

        return this;
    }

    public GraphBuilder Definition(Action<DefinitionBuilder> definition)
    {
        var builder = new DefinitionBuilder();
        definition(builder);

        _definitions.Add(builder);

        return this;
    }

    public GraphBuilder Definition(string variableName, string fileName, string dataSourceName, ConsolidationFunction consolidationFunction = ConsolidationFunction.Average,
        int? step = null, string? start = null, string? end = null, ConsolidationFunction? reduce = null)
    {
        var definition = new DefinitionBuilder(variableName, fileName, dataSourceName, consolidationFunction, step, start, end, reduce);

        _definitions.Add(definition);

        return this;
    }

    public GraphBuilder Definition(string fileName, ConsolidationFunction consolidationFunction = ConsolidationFunction.Average,
        int? step = null, string? start = null, string? end = null, ConsolidationFunction? reduce = null)
    {
        var definition = new DefinitionBuilder(fileName, consolidationFunction, step, start, end, reduce);

        _definitions.Add(definition);

        return this;
    }

    public GraphBuilder Print(string variableName, string format, string? str = null)
    {
        if (string.IsNullOrEmpty(variableName))
            throw new ArgumentException("Argument not set", nameof(variableName));
        if (string.IsNullOrEmpty(variableName))
            throw new ArgumentException("Argument not set", nameof(format));

        var sb = new StringBuilder("PRINT:").Append(variableName).Append(':').Append(format);

        if (str != null)
            sb.Append(':').Append(str);

        _prints ??= [];
        _prints.Add(sb.ToString());

        return this;
    }

    public GraphBuilder GPrint(string variableName, string format)
    {
        if (string.IsNullOrEmpty(variableName))
            throw new ArgumentException("Argument not set", nameof(variableName));
        if (string.IsNullOrEmpty(variableName))
            throw new ArgumentException("Argument not set", nameof(format));

        _gprints ??= [];
        _gprints.Add($"GPRINT:{variableName}:{format}");

        return this;
    }

    public GraphBuilder Comment(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Argument not set", nameof(text));

        _comments ??= [];
        _comments.Add($"COMMENT:{text}");

        return this;
    }

    public GraphBuilder Line(int? width, string value, string? color = "#0000FF", string? legend = null, bool stack = false)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Argument not set", nameof(value));

        var sb = new StringBuilder("LINE");

        if (width.HasValue)
            sb.Append(width);

        sb.Append(':').Append(value);

        if (!string.IsNullOrEmpty(color))
        {
            if (!color.StartsWith('#'))
                sb.Append('#');

            sb.Append(color);
        }

        if (!string.IsNullOrEmpty(legend))
            sb.Append(':').Append(legend);

        _lines ??= [];
        _lines.Add(sb.ToString());

        return this;
    }

    public GraphBuilder Area(string value, string? color = null, string? legend = null, bool stack = false)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Argument not set", nameof(value));

        var sb = new StringBuilder("AREA:").Append(value);

        if (!string.IsNullOrEmpty(color))
        {
            if (!color.StartsWith('#'))
                sb.Append('#');

            sb.Append(color);
        }

        if (!string.IsNullOrEmpty(legend))
            sb.Append(':').Append(legend);

        _areas ??= [];
        _areas.Add(sb.ToString());

        return this;
    }

    public GraphBuilder Tick(string variableName, string color, double? fraction = null, string? legend = null)
    {
        if (string.IsNullOrEmpty(variableName))
            throw new ArgumentException("Argument not set", nameof(variableName));
        if (string.IsNullOrEmpty(color))
            throw new ArgumentException("Argument not set", nameof(color));

        var sb = new StringBuilder("TICK:").Append(variableName);

        if (!color.StartsWith('#'))
            sb.Append('#');

        sb.Append(color);

        if (fraction.HasValue)
        {
            sb.Append(':').Append(fraction);

            if (!string.IsNullOrEmpty(legend))
                sb.Append(':').Append(legend);
        }

        _ticks ??= [];
        _ticks.Add(sb.ToString());

        return this;
    }

    public GraphBuilder Shift(string variableName, int offset)
    {
        if (string.IsNullOrEmpty(variableName))
            throw new ArgumentException("Argument not set", nameof(variableName));

        _shifts ??= [];
        _shifts.Add($"SHIFT:{variableName}:{offset}");

        return this;
    }

    public GraphBuilder TextAlign(TextAlign textAlign)
    {
        _textaligns ??= [];
        _textaligns.Add($"TEXTALIGN:{textAlign.ToString().ToLower()}");

        return this;
    }

    internal List<string> Build()
    {
        var arguments = new List<string>();

        if (_fileName == null)
            throw new InvalidOperationException("FileName is required");
        if (_definitions.Count == 0)
            throw new InvalidOperationException("At least one definition is required");

        var name = Path.GetFileName(_fileName);

        if (!name.Contains('.'))
            _fileName = string.Concat(_fileName, '.', _imageFormat.ToString().ToLower());

        Add(arguments, "graph", _fileName);
        Add(arguments, "-s", _start);
        Add(arguments, "-e", _end);
        Add(arguments, "-S", _step);
        Add(arguments, "-t", _title);
        Add(arguments, "-v", _verticalLabel);
        Add(arguments, "-w", _width);
        Add(arguments, "-h", _height);
        Add(arguments, "-j", _onlyGraph);
        Add(arguments, "-D", _fullSizeMode);
        Add(arguments, "-u", _upperLimit);
        Add(arguments, "-l", _lowerLimit);
        Add(arguments, "-r", _rigid);
        Add(arguments, "--allow-shrink", _allowShrink);
        Add(arguments, "-a", _imageFormat.ToString().ToUpper());
        Add(arguments, "--disable-rrdtool-tag", _disableRrdtoolTag);

        // start data section

        foreach (var definition in _definitions)
        {
            arguments.Add(definition.Build());
        }

        // start graph section

        Add(arguments, _prints);
        Add(arguments, _gprints);
        Add(arguments, _comments);
        Add(arguments, _lines);
        Add(arguments, _areas);
        Add(arguments, _ticks);
        Add(arguments, _shifts);
        Add(arguments, _textaligns);

        return arguments;
    }
}
