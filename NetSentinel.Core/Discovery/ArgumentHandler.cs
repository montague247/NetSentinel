using NetSentinel.ArgumentHandling;
using NetSentinel.Nmap;

namespace NetSentinel.Discovery
{
    [ArgumentHandler("--discovery")]
    public sealed class ArgumentHandler : ArgumentHandlerBase, IDiscoveryOptions
    {
        private List<string>? _ipRanges;
        private bool _keepTempFiles;

        public bool KeepTempFiles => _keepTempFiles;

        protected override Dictionary<string, string> Help => new()
        {
            { "--ip", "IP or IP range" },
            { "--keep-temp-files", "Keeps all tempory files" }
        };

        public override void Execute()
        {
            if (_ipRanges == null)
            {
                NmapExecute.ArpPingScan("192.168.1.1/16", this);

                return;
            }

            foreach (var ipRange in _ipRanges)
                NmapExecute.ArpPingScan(ipRange, this);
        }

        protected override bool Process(string argument, string[] arguments, ref int index)
        {
            switch (argument)
            {
                case "--ip":
                    _ipRanges ??= [];
                    _ipRanges.AddRange(arguments[++index].Split(','));
                    return true;
                case "--keep-temp-files":
                    _keepTempFiles = true;
                    return true;
                default:
                    return false;
            }
        }
    }
}
