using NetSentinel.ArgumentHandling;
using NetSentinel.Nmap;

namespace NetSentinel.Discovery
{
    [ArgumentHandler("--discovery", "Run discovery scan")]
    public sealed class ArgumentHandler : ArgumentHandlerBase, IDiscoveryOptions
    {
        [ArgumentHandlerProperty("--ip", "IP or IP range (comma-separated)")]
        [ArgumentHandlerPropertyValue("ipOrRange", "192.168.1.1/20", "Default IP range for ARP ping scan")]
        [ArgumentHandlerPropertyValue("ipOrRange", "192.168.4.1", "Alternative IP for ARP ping scan")]
        public List<string>? IpRanges { get; private set; }

        [ArgumentHandlerProperty("--keep-temp-files", "Keeps all temporary files")]
        public bool KeepTempFiles { get; private set; }

        public override void Execute(IGlobalOptions options)
        {
            if (IpRanges == null)
            {
                NmapExecute.ArpPingScan("192.168.1.1/20", this, options);

                return;
            }

            foreach (var ipRange in IpRanges)
                NmapExecute.ArpPingScan(ipRange, this, options);
        }

        protected override bool Process(string argument, string[] arguments, ref int index)
        {
            switch (argument)
            {
                case "--ip":
                    IpRanges ??= [];
                    IpRanges.AddRange(arguments[++index].Split(','));
                    return true;
                case "--keep-temp-files":
                    KeepTempFiles = true;
                    return true;
                default:
                    return false;
            }
        }
    }
}
