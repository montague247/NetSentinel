using System.Globalization;

namespace NetSentinel.RrdTool
{
    public sealed class GraphBuilder : ArgumentsBuilder
    {
        private string? _fileName;
        private string? _start = "now-1h";
        private string? _end = "now";
        private int _step = 1;
        private string? _title;
        private string? _verticalLabel;
        private int _width = 400;
        private int _height = 100;
        private bool _onlyGraph;
        private bool _fullSizeMode;
        private ImageFormat _imageFormat;

        public GraphBuilder FileName(string fileName)
        {
            _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));

            return this;
        }

        public GraphBuilder Start(string start)
        {
            _start = start ?? throw new ArgumentNullException(nameof(start));

            return this;
        }

        public GraphBuilder End(string end)
        {
            _end = end ?? throw new ArgumentNullException(nameof(end));

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

        public GraphBuilder ImageFormat(ImageFormat imageFormat)
        {
            _imageFormat = imageFormat;

            return this;
        }

        internal List<string> Build()
        {
            var arguments = new List<string>();

            if (_fileName == null)
                throw new InvalidOperationException("FileName is required");

            var name = Path.GetFileName(_fileName);

            if (!name.Contains('.'))
                _fileName = string.Concat(_fileName, '.', _imageFormat.ToString().ToLower());

            Add(arguments, "graph", _fileName);
            Add(arguments, "-s", _start);
            Add(arguments, "-e", _end);
            Add(arguments, "-S", _step.ToString(CultureInfo.InvariantCulture));
            Add(arguments, "-t", _title);
            Add(arguments, "-v", _verticalLabel);
            Add(arguments, "-w", _width.ToString(CultureInfo.InvariantCulture));
            Add(arguments, "-h", _height.ToString(CultureInfo.InvariantCulture));
            Add(arguments, "-j", _onlyGraph);
            Add(arguments, "-D", _fullSizeMode);
            Add(arguments, "-a", _imageFormat.ToString().ToUpper());

            return arguments;
        }
    }
}
