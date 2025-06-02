using System.Globalization;

namespace NetSentinel.RrdTool;

public sealed class UpdateBuilder : ArgumentsBuilder
{
    private string? _fileName;
    private string _timestamp = "N";
    private string[]? _values;

    public UpdateBuilder FileName(string fileName)
    {
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

        return this;
    }

    public UpdateBuilder Timestamp(string timestamp)
    {
        _timestamp = timestamp ?? throw new ArgumentNullException(nameof(timestamp));

        return this;
    }

    public UpdateBuilder Timestamp(int timestamp)
    {
        _timestamp = timestamp.ToString(CultureInfo.InvariantCulture);

        return this;
    }

    public UpdateBuilder Timestamp(DateTime dateTime)
    {
        Timestamp((int)(dateTime - DateTime.UnixEpoch).TotalSeconds);

        return this;
    }

    public UpdateBuilder Value(params double[] value)
    {
        _values = value.Select(s => double.IsNaN(s) ? "U" : s.ToString(CultureInfo.InvariantCulture)).ToArray();

        return this;
    }

    internal List<string> Build()
    {
        var arguments = new List<string>();

        if (_fileName == null)
            throw new InvalidOperationException("FileName is required");
        if (_values == null)
            throw new InvalidOperationException("Value is required");

        Add(arguments, "update", _fileName);

        arguments.Add($"{_timestamp}:{string.Join(':', _values)}");

        return arguments;
    }
}
